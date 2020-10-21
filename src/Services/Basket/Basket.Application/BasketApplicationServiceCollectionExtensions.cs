using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Assemblers.Implementations;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Application
{
    [ExcludeFromCodeCoverage]
    public static class BasketApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddBasketApplicationComponents(this IServiceCollection services)
        {
            services.AddScoped<IMoneyAssembler, MoneyAssembler>();
            services.AddScoped<IBasketAssembler, BasketAssembler>();
            services.AddScoped<IBasketItemAssembler, BasketItemAssembler>();
            services.AddBasketInfrastructureComponents();
            return services;
        }
    }
}