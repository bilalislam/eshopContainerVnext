using System;

namespace Basket.Domain.Aggregates.Entities
{
    public class BasketItem : EntityBase
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal OldUnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string PictureUrl { get; private set; }

        public BasketItem()
        {
        }

        public BasketItem(Guid id) : base(id)
        {
        }

        public static BasketItem Load()
        {
            return new BasketItem();
        }
    }
}