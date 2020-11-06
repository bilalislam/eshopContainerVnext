using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Basket.API.Model;
using Basket.Domain.Contracts;

namespace Basket.FunctionalTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class FakeDataGenerator
    {
        public static BasketCheckout CreateBasketCheckout(string id)
        {
            return new BasketCheckout()
            {
                Buyer = "bilal",
                City = "Istanbul",
                Country = "Turkiye",
                State = "Umraniye",
                Street = "Talatpasa cad.",
                BuyerId = id,
                CardExpiration = DateTime.Today,
                CardNumber = "1231232123123",
                RequestId = Guid.NewGuid(),
                ZipCode = "34704",
                CardHolderName = "bilal islam",
                CardSecurityNumber = "821",
                CardTypeId = 1
            };
        }

        public static BasketContract CreateBasketContract(string id)
        {
            return new BasketContract()
            {
                Id = Guid.NewGuid(),
                BuyerId = id,
                BasketItems = new List<BasketItemContract>()
                {
                    new BasketItemContract()
                    {
                        Id = Guid.NewGuid(),
                        Quantity = 1,
                        PictureUrl = "test",
                        ProductId = 12,
                        ProductName = "çanta",
                        UnitPrice = new MoneyContract()
                        {
                            Amount = 12
                        },
                        OldUnitPrice = new MoneyContract()
                        {
                            Amount = 10
                        }
                    }
                }
            };
        }
    }
}