using System.Threading;
using System.Threading.Tasks;

namespace Basket.Domain.RepositoryInterfaces
{
    public interface IBasketCommandRepository
    {
        Task SaveAsync(Aggregates.Basket basket, CancellationToken cancellationToken);
        Task DeleteAsync(string buyerId, CancellationToken cancellationToken);
    }
}