using System.Diagnostics.CodeAnalysis;
using Basket.Domain.RepositoryInterfaces;
using Basket.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class BasketInfrastructureServiceCollectionExtension
    {
        public static IServiceCollection AddBasketInfrastructureComponents(this IServiceCollection services)
        {
            services.AddScoped<IBasketQueryRepository, BasketQueryRepository>();
            return services;
        }
    }
}