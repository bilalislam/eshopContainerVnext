using System;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.UnitTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Basket.UnitTests.Entities
{
    [TestFixture]
    public class BasketItemTests
    {
        [Test]
        public void Load_ShouldThrowException_WhenDomainDtoIsNull()
        {
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(null); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1008));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductIdEqualThanZero()
        {
            //Arrange
            var basketItem = FakeDataGenerator.CreateBasketItemContract();
            basketItem.ProductId = 0;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1009));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductIdLessThanZero()
        {
            //Arrange
            var basketItem = FakeDataGenerator.CreateBasketItemContract();
            basketItem.ProductId = -1;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1009));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductNameIsNullOrEmpty()
        {
            //Arrange
            var basketItem = FakeDataGenerator.CreateBasketItemContract();
            basketItem.ProductName = string.Empty;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1005));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductQuantityEqualToZero()
        {
            //Arrange
            var basketItem = FakeDataGenerator.CreateBasketItemContract();
            basketItem.Quantity = 0;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1006));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductQuantityLessThanZero()
        {
            //Arrange
            var basketItem = FakeDataGenerator.CreateBasketItemContract();
            basketItem.Quantity = -1;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1006));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductUrlNullOrEmpty()
        {
            //Arrange
            var basketItem = FakeDataGenerator.CreateBasketItemContract();
            basketItem.PictureUrl = String.Empty;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1007));
        }
    }
}