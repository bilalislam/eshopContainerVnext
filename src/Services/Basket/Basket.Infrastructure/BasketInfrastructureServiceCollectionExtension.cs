using System.Diagnostics.CodeAnalysis;
using Basket.Domain.RepositoryInterfaces;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Repositories.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Basket.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class BasketInfrastructureServiceCollectionExtension
    {
        /// <summary>
        /// https://aws.amazon.com/builders-library/timeouts-retries-and-backoff-with-jitter/
        /// all proxies must contains resilience concepts
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBasketInfrastructureComponents(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddRedisCache();
            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBasketQueryRepository, BasketQueryRepository>();
            services.AddScoped<IBasketCommandRepository, BasketCommandRepository>();
        }
        
        private static void AddRedisCache(this IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var settings = config.GetSection("redis").Get<RedisSettings>();
                services.AddSingleton(sp =>
                {
                    var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);
                    configuration.ResolveDns = true;
                    return ConnectionMultiplexer.Connect(configuration);
                });
            }
        }
    }
}