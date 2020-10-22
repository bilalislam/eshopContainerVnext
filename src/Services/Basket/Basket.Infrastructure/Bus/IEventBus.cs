using System.Threading.Tasks;
using Basket.Domain.Events;

namespace Basket.Infrastructure.Bus
{
    public interface IEventBus
    {
        Task<bool> PublishMessageAsync<TMessage>(TMessage msg, string exchangeName, string exchangeType,
            string routingKey) where TMessage : IEvent;
    }
}