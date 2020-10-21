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

        public async Task SaveAsync(Domain.Aggregates.Basket basket, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"basket with {basket.BuyerId} saved");
            await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
        }

        public async Task DeleteAsync(string buyerId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"basket with {buyerId} deleted");
            await _database.KeyDeleteAsync(buyerId);
        }
    }
}