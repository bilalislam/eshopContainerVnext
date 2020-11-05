﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopOnContainers.Services.Basket.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.FunctionalTests.Base
{
    class BasketTestsStartup : Startup
    {
        public BasketTestsStartup(IConfiguration env) : base(env)
        {
        }

        // public void ConfigureServices(IServiceCollection services)
        // {
        //     // Added to avoid the Authorize data annotation in test environment. 
        //     // Property "SuppressCheckForUnhandledSecurityMetadata" in appsettings.json
        //     services.Configure<RouteOptions>(Configuration);
        // }


        protected override void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {
                app.UseMiddleware<AutoAuthorizeMiddleware>();
            }
            else
            {
                base.ConfigureAuth(app);
            }
        }
    }
}