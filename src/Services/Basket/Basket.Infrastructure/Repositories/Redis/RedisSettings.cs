using System.Diagnostics.CodeAnalysis;

namespace Basket.Infrastructure.Repositories.Redis
{
    [ExcludeFromCodeCoverage]
    public class RedisSettings
    {
        public string ConnectionString { get; set; }
    }
}