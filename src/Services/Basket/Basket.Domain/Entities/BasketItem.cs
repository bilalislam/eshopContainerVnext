using System;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.Domain.Shared;
using Basket.Domain.ValueObjects;

namespace Basket.Domain.Entities
{
    public class BasketItem : EntityBase<Guid>
    {
        //For example ; this field can keep as Identity for product entity but not necessary right now.
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money OldUnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string PictureUrl { get; private set; }

        private BasketItem() : base(Guid.NewGuid())
        {
        }

        public static BasketItem Load(int productId, string productName, decimal unitPrice, decimal oldUnitPrice,
            int quantity, string pictureUrl)
        {
            Guard.That<DomainException>(productId <= 0, nameof(DomainErrorCodes.EDBasket1004),
                DomainErrorCodes.EDBasket1004);

            Guard.That<DomainException>(string.IsNullOrEmpty(productName), nameof(DomainErrorCodes.EDBasket1005),
                DomainErrorCodes.EDBasket1005);

            Guard.That<DomainException>(quantity <= 0, nameof(DomainErrorCodes.EDBasket1006),
                DomainErrorCodes.EDBasket1006);

            Guard.That<DomainException>(string.IsNullOrEmpty(pictureUrl), nameof(DomainErrorCodes.EDBasket1007),
                DomainErrorCodes.EDBasket1007);

            return new BasketItem
            {
                ProductId = productId,
                ProductName = productName,
                UnitPrice = Money.Load(unitPrice),
                OldUnitPrice = Money.Load(oldUnitPrice),
                Quantity = quantity,
                PictureUrl = pictureUrl
            };
        }
    }
}