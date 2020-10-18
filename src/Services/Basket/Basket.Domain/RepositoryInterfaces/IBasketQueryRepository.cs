using System.Threading.Tasks;

namespace Basket.Domain.RepositoryInterfaces
{
    public interface IBasketQueryRepository
    {
        Task<Aggregates.Basket> GetBasketAsync(string id);
    }
}