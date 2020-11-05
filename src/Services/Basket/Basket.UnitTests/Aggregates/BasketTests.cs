using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Basket.Domain.Contracts;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Events;
using Basket.Domain.Exceptions;
using Basket.UnitTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Basket.UnitTests.Aggregates
{
    [TestFixture]
    public class BasketTests
    {
        [Test]
        public void Load_ShouldThrowException_WhenDomainDtoIsNull()
        {
            // Act
            Action basket = () => { Domain.Aggregates.Basket.Load(null); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1000));
        }

        [Test]
        public void Load_ShouldThrowException_WhenBuyerIsNull()
        {
            //Arrange
            var updateBasketCommand = FakeDataGenerator.CreateBasketCommand();
            updateBasketCommand.BuyerId = null;

            // Act
            Action basket = () => { Domain.Aggregates.Basket.Load(updateBasketCommand); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1001));
        }

        [Test]
        public void Load_ShouldCreateBasket_WhenBasketDoesValid()
        {
            //Arrange
            var updateBasketCommand = FakeDataGenerator.CreateBasketCommand();

            // Act
            var basket = Domain.Aggregates.Basket.Load(updateBasketCommand);

            // Assert
            basket.GetUncommittedEvents().First().Should().BeOfType<BasketCreated>();
        }

        [Test]
        public void AddCartItem_ShouldThrowException_WhenBasketItemsNullOrEmpty()
        {
            //Arrange
            var updateBasketCommand = FakeDataGenerator.CreateBasketCommand();
            updateBasketCommand.Items = null;

            // Act
            Action basket = () =>
            {
                Domain.Aggregates.Basket.Load(updateBasketCommand)
                    .AddCartItem(updateBasketCommand.Items);
            };

            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1008));
        }

        [Test]
        public void AddCartItem_ShouldAddProductsToBasket_WhenBasketItemsAreValid()
        {
            //Arrange
            var updateBasketCommand = FakeDataGenerator.CreateBasketCommand();

            // Act
            var basket = Domain.Aggregates.Basket.Load(updateBasketCommand);

            basket.AddCartItem(new List<BasketItemContract>()
            {
                FakeDataGenerator.CreateBasketItemContract()
            });

            // Assert
            basket.Items.Should().HaveCount(1);
        }
    }
}