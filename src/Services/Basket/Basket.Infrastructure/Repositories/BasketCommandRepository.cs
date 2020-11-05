using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.Infrastructure.Repositories
{
    /// <summary>
    /// TODO: raise all events
    /// </summary>
    public class BasketCommandRepository : IBasketCommandRepository
    {
        private readonly ILogger<BasketQueryRepository> _logger;
        private readonly IDatabase _database;

        public BasketCommandRepository(ILogger<BasketQueryRepository> logger,
            ConnectionMultiplexer redis)
        {
            _logger = logger;
            _database = redis.GetDatabase();
        }

        public async Task<bool> SaveAsync(Domain.Aggregates.Basket basket, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"basket with {basket.BuyerId} saved");
            return await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
        }

        public async Task<bool> DeleteAsync(string buyerId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"basket with {buyerId} deleted");
            return await _database.KeyDeleteAsync(buyerId);
        }
    }
}