using System.Threading.Tasks;
using Basket.Domain.RepositoryInterfaces;

namespace Basket.Infrastructure.Repositories
{
    public class BasketQueryRepository : IBasketQueryRepository
    {
        public Task<Domain.Aggregates.Basket> GetBasketAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}