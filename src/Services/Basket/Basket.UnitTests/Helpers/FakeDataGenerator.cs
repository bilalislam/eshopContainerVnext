using System.Collections.Generic;
using System.Linq;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using Bogus;

namespace Basket.UnitTests.Helpers
{
    public static class FakeDataGenerator
    {
        private static readonly Faker faker = new Faker();

        public static UpdateBasketCommand GetBasketCommand()
        {
            return new UpdateBasketCommand()
            {
                BuyerId = faker.Random.String(),
                Items = faker.Random.ListItems(new List<BasketItemContract>()
                {
                    new BasketItemContract()
                    {
                        Quantity = 1,
                        PictureUrl = faker.Random.String(),
                        ProductId = faker.Random.Int(),
                        ProductName = faker.Random.String(),
                        UnitPrice = new MoneyContract()
                        {
                            Amount = faker.Random.Decimal()
                        },
                        OldUnitPrice = new MoneyContract()
                        {
                            Amount = faker.Random.Decimal()
                        },
                    }
                }).ToList()
            };
        }
    }
}