using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Entities;

namespace Basket.Domain.Events
{
    [ExcludeFromCodeCoverage]
    public class BasketItemAdded : EventBase
    {
        public BasketItem BasketItem { get; private set; }
        public string BuyerId { get; private set; }

        public BasketItemAdded(string buyerId, BasketItem basketItem)
        {
            BuyerId = buyerId;
            BasketItem = basketItem;
        }
    }
}