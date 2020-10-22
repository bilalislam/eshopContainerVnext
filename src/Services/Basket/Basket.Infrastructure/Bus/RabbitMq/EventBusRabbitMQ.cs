using System.IO;
using System.Threading.Tasks;
using Basket.Domain.Events;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Basket.Infrastructure.Bus.RabbitMq
{
    public class EventBusRabbitMq : IEventBus
    {
        private readonly ObjectPool<IConnection> _connectionPool;

        public EventBusRabbitMq(IPooledObjectPolicy<IConnection> objectPolicy)
        {
            _connectionPool = new DefaultObjectPool<IConnection>(objectPolicy);
        }

        public Task<bool> PublishMessageAsync<TMessage>(TMessage msg, string exchangeName, string exchangeType,
            string routingKey) where TMessage : IEvent
        {
            var conn = _connectionPool.Get();
            try
            {
                using (var model = conn.CreateModel())
                {
                    var body = Serialize(msg);
                    var props = model.CreateBasicProperties();
                    props.Persistent = true;

                    model.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                    model.BasicPublish(exchangeName, routingKey, false, props, body);

                    return Task.FromResult(true);
                }
            }
            finally
            {
                _connectionPool.Return(conn);
            }
        }

        private byte[] Serialize<T>(T value)
        {
            using (var ms = new MemoryStream())
            {
                using (var sr = new StreamWriter(ms))
                using (var jtr = new JsonTextWriter(sr))
                {
                    new JsonSerializer().Serialize(jtr, value);
                }

                return ms.ToArray();
            }
        }
    }
}