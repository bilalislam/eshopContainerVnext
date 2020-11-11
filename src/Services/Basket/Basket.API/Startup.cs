using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using Basket.API.Assemblers.Implementations;
using Basket.API.Assemblers.Interfaces;
using Basket.API.Controllers;
using Basket.API.Infrastructure.Filters;
using Basket.API.Infrastructure.Middlewares;
using Basket.API.IntegrationEvents.EventHandling;
using Basket.API.IntegrationEvents.Events;
using Basket.Application;
using GrpcBasket;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ;
using Microsoft.eShopOnContainers.Services.Basket.API.Infrastructure.Repositories;
using Microsoft.eShopOnContainers.Services.Basket.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Basket.API.Model;
using Microsoft.eShopOnContainers.Services.Basket.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Basket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBasketAssembler, BasketAssembler>();
            services.AddMediatR(Assembly.Load("Basket.Application"));
            services.AddBasketApplicationComponents();


            services.AddGrpc(options => { options.EnableDetailedErrors = true; });

            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(ValidateModelStateFilter));
                }) // Added for functional tests
                .AddApplicationPart(typeof(BasketController).Assembly)
                .AddNewtonsoftJson();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eShopOnContainers - Basket HTTP API",
                    Version = "v1",
                    Description = "The Basket Service HTTP API"
                });

                // options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                // {
                //     Type = SecuritySchemeType.OAuth2,
                //     Flows = new OpenApiOAuthFlows()
                //     {
                //         Implicit = new OpenApiOAuthFlow()
                //         {
                //             AuthorizationUrl =
                //                 new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                //             TokenUrl =
                //                 new Uri($"{Configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                //             Scopes = new Dictionary<string, string>()
                //             {
                //                 {"basket", "Basket API"}
                //             }
                //         }
                //     }
                // });
                //
                // options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            //ConfigureAuthService(services);

            services.AddCustomHealthCheck(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IBasketRepository, RedisBasketRepository>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseSwagger()
                .UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint(
                        $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json",
                        "Basket.API V1");
                    // setup.OAuthClientId("basketswaggerui");
                    // setup.OAuthAppName("Basket Swagger UI");
                });

            app.UseRouting();
            app.UseCors("CorsPolicy");
            //ConfigureAuth(app);

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<BasketService>();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapGet("/_proto/", async ctx =>
                {
                    ctx.Response.ContentType = "text/plain";
                    using var fs = new FileStream(Path.Combine(env.ContentRootPath, "Proto", "basket.proto"),
                        FileMode.Open, FileAccess.Read);
                    using var sr = new StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        if (line != "/* >>" || line != "<< */")
                        {
                            await ctx.Response.WriteAsync(line);
                        }
                    }
                });
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "basket";
            });
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services,
            IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddRedis(
                    configuration["ConnectionString"],
                    name: "redis-check",
                    tags: new string[] {"redis"});

            hcBuilder
                .AddRabbitMQ(
                    $"amqp://{configuration["EventBusConnection"]}",
                    name: "basket-rabbitmqbus-check",
                    tags: new string[] {"rabbitmqbus"});

            return services;
        }
    }
}