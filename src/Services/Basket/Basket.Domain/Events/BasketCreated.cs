using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Events
{
    [ExcludeFromCodeCoverage]
    public class BasketCreated : EventBase
    {
        public string BuyerId { get; private set; }

        public BasketCreated(string buyerId)
        {
            BuyerId = buyerId;
        }
    }
}