using System;
using Basket.Domain.Contracts;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.Domain.Shared;
using Basket.Domain.ValueObjects;
using MediatR;

namespace Basket.Domain.Entities
{
    public class BasketItem : EntityBase<Guid>
    {
        //For example ; this field can keep as Identity for product entity but not necessary right now.
        //https://github.com/VaughnVernon/IDDD_Samples_NET/blob/90fcc52d9c1af29640ec2a8a3e0e7c692f3e6663/iddd_agilepm/Domain.Model/Products/ProductId.cs
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money OldUnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string PictureUrl { get; private set; }

        //Impedance Mismatch :(
        public BasketItem(int productId, string productName, Money unitPrice, Money oldUnitPrice, int quantity,
            string pictureUrl) : base(Guid.NewGuid())
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            OldUnitPrice = oldUnitPrice;
            Quantity = quantity;
            PictureUrl = pictureUrl;
        }

        public static BasketItem Load(BasketItemContract basketItemContract)
        {
            Guard.That<DomainException>(basketItemContract.ProductId <= 0, nameof(DomainErrorCodes.EDBasket1009),
                DomainErrorCodes.EDBasket1009);

            Guard.That<DomainException>(string.IsNullOrEmpty(basketItemContract.ProductName),
                nameof(DomainErrorCodes.EDBasket1005),
                DomainErrorCodes.EDBasket1005);

            Guard.That<DomainException>(basketItemContract.Quantity <= 0, nameof(DomainErrorCodes.EDBasket1006),
                DomainErrorCodes.EDBasket1006);

            Guard.That<DomainException>(string.IsNullOrEmpty(basketItemContract.PictureUrl),
                nameof(DomainErrorCodes.EDBasket1007),
                DomainErrorCodes.EDBasket1007);

            return new BasketItem(
                basketItemContract.ProductId,
                basketItemContract.ProductName,
                Money.Load(basketItemContract.UnitPrice.Amount),
                Money.Load(basketItemContract.OldUnitPrice.Amount),
                basketItemContract.Quantity,
                basketItemContract.PictureUrl);
        }
    }
}