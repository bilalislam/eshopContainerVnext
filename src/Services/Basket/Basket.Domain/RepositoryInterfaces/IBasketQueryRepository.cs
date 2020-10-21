using System.Threading;
using System.Threading.Tasks;

namespace Basket.Domain.RepositoryInterfaces
{
    public interface IBasketQueryRepository
    {
        Task<Aggregates.Basket> GetBasketAsync(string id, CancellationToken cancellationToken);
    }
}