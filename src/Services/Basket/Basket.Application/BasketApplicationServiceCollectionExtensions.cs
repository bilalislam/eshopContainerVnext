using System.Diagnostics.CodeAnalysis;
using Basket.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Application
{
    [ExcludeFromCodeCoverage]
    public static class BasketApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddBasketApplicationComponents(this IServiceCollection services)
        {
            services.AddBasketInfrastructureComponents();
            return services;
        }
    }
}