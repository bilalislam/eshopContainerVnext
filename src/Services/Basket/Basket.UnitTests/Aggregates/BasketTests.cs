using System;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.ErrorCodes;
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
            var updateBasketCommand = FakeDataGenerator.GetBasketCommand();
            updateBasketCommand.BuyerId = null;

            // Act
            Action basket = () => { Domain.Aggregates.Basket.Load(updateBasketCommand); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1001));
        }

        [Test]
        public void Load_ShouldThrowException_WhenBasketItemsNullOrEmpty()
        {
            //Arrange
            var updateBasketCommand = FakeDataGenerator.GetBasketCommand();
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
    }
}