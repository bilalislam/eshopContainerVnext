using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Polly;

namespace Basket.Infrastructure.Bus.RabbitMq
{
    [ExcludeFromCodeCoverage]
    public class ConnectionPooledObjectPolicy : IPooledObjectPolicy<IConnection>
    {
        private readonly ILogger<ConnectionPooledObjectPolicy> _logger;

        public ConnectionPooledObjectPolicy(ILogger<ConnectionPooledObjectPolicy> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Add a jitter or polly strategy to the retry policy
        /// https://aws.amazon.com/builders-library/timeouts-retries-and-backoff-with-jitter/
        /// </summary>
        /// <returns></returns>
        public IConnection Create()
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                UseBackgroundThreadsForIO = false
            };

            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.LogWarning(ex,
                            "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
                            $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

            IConnection conn = null;
            policy.Execute(() => { conn = factory.CreateConnection(); });

            return conn;
        }

        public bool Return(IConnection obj)
        {
            return true;
        }
    }
}