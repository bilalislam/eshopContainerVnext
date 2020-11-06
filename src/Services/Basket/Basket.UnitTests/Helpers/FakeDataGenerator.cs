using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using Bogus;

namespace Basket.UnitTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class FakeDataGenerator
    {
        private static readonly Faker Faker = new Faker();

        public static UpdateBasketCommand CreateBasketCommand()
        {
            return new UpdateBasketCommand()
            {
                BuyerId = Faker.Random.String(),
                Items = new List<BasketItemContract>()
                {
                    new BasketItemContract()
                    {
                        Quantity = 1,
                        PictureUrl = Faker.Random.String(),
                        ProductId = Faker.Random.Int(),
                        ProductName = Faker.Random.String(),
                        UnitPrice = new MoneyContract()
                        {
                            Amount = Faker.Random.Decimal()
                        },
                        OldUnitPrice = new MoneyContract()
                        {
                            Amount = Faker.Random.Decimal()
                        },
                    }
                }
            };
        }

        public static BasketItemContract CreateBasketItemContract()
        {
            return new BasketItemContract()
            {
                Quantity = 1,
                PictureUrl = Faker.Random.String(),
                ProductId = Faker.Random.Int(1),
                ProductName = Faker.Random.String(),
                UnitPrice = new MoneyContract()
                {
                    Amount = Faker.Random.Decimal(1)
                },
                OldUnitPrice = new MoneyContract()
                {
                    Amount = Faker.Random.Decimal(1)
                },
            };
        }

        public static BasketContract CreateBasketContract()
        {
            return new BasketContract()
            {
                Id = Faker.Random.Guid(),
                BuyerId = Faker.Random.String(),
                BasketItems = new[]
                {
                    CreateBasketItemContract()
                }
            };
        }

        public static Domain.Aggregates.Basket CreateBasket()
        {
            return Domain.Aggregates.Basket.Load(CreateBasketCommand());
        }
    }
}