using System;
using Basket.Domain.Contracts;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.Domain.RepositoryInterfaces;
using Basket.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Basket.UnitTests.Entities
{
    [TestFixture]
    public class BasketItemTests
    {
        private BasketItemContract _basketItem;

        [SetUp]
        public void Init()
        {
            _basketItem = FakeDataGenerator.CreateBasketItemContract();
        }

        [TearDown]
        public void Dispose()
        {
            _basketItem = null;
        }


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
            _basketItem.ProductId = 0;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(_basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1009));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductIdLessThanZero()
        {
            //Arrange
            _basketItem.ProductId = -1;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(_basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1009));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductNameIsNullOrEmpty()
        {
            //Arrange
            _basketItem.ProductName = string.Empty;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(_basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1005));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductQuantityEqualToZero()
        {
            //Arrange
            _basketItem.Quantity = 0;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(_basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1006));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductQuantityLessThanZero()
        {
            //Arrange
            _basketItem.Quantity = -1;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(_basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1006));
        }

        [Test]
        public void Load_ShouldThrowException_WhenProductUrlNullOrEmpty()
        {
            //Arrange
            _basketItem.PictureUrl = String.Empty;
            // Act
            Action basket = () => { Domain.Entities.BasketItem.Load(_basketItem); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1007));
        }
    }
}