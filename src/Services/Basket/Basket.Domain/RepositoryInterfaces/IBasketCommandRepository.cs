using System.Threading;
using System.Threading.Tasks;

namespace Basket.Domain.RepositoryInterfaces
{
    public interface IBasketCommandRepository
    {
        Task<bool> SaveAsync(Aggregates.Basket basket, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string buyerId, CancellationToken cancellationToken);
    }
}