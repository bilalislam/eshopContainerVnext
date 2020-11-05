using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BasketQueryRepository : IBasketQueryRepository
    {
        private readonly ILogger<BasketQueryRepository> _logger;
        private readonly IDatabase _database;

        public BasketQueryRepository(ILogger<BasketQueryRepository> logger, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _database = redis.GetDatabase();
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Object%E2%80%93relational_impedance_mismatch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Domain.Aggregates.Basket> GetBasketAsync(string id, CancellationToken cancellationToken)
        {
            var data = await _database.StringGetAsync(id);
            if (!data.IsNullOrEmpty) return JsonConvert.DeserializeObject<Domain.Aggregates.Basket>(data);
            _logger.LogError($"Basket with {id} could not be found !");
            return null;
        }
    }
}