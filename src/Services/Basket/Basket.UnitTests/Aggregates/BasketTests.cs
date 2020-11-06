using System;
using System.Collections.Generic;
using System.Linq;
using Basket.Domain.Commands.UpdateBasket;
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
        private UpdateBasketCommand _updateBasketCommand;

        [SetUp]
        public void Init()
        {
            _updateBasketCommand = FakeDataGenerator.CreateBasketCommand();
        }

        [TearDown]
        public void Dispose()
        {
            _updateBasketCommand = null;
        }


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
            _updateBasketCommand.BuyerId = null;

            // Act
            Action basket = () => { Domain.Aggregates.Basket.Load(_updateBasketCommand); };
            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1001));
        }

        [Test]
        public void Load_ShouldCreateBasket_WhenBasketDoesValid()
        {
            // Act
            var basket = Domain.Aggregates.Basket.Load(_updateBasketCommand);

            // Assert
            basket.GetUncommittedEvents().First().Should().BeOfType<BasketCreated>();
        }

        [Test]
        public void AddCartItem_ShouldThrowException_WhenBasketItemsNullOrEmpty()
        {
            //Arrange
            _updateBasketCommand.Items = null;

            // Act
            Action basket = () =>
            {
                Domain.Aggregates.Basket.Load(_updateBasketCommand)
                    .AddCartItem(_updateBasketCommand.Items);
            };

            // Assert
            basket.Should().Throw<DomainException>().And.Code.Should().Be(nameof(DomainErrorCodes.EDBasket1008));
        }

        [Test]
        public void AddCartItem_ShouldAddProductsToBasket_WhenBasketItemsAreValid()
        {
            // Act
            var basket = Domain.Aggregates.Basket.Load(_updateBasketCommand);

            basket.AddCartItem(new List<BasketItemContract>()
            {
                FakeDataGenerator.CreateBasketItemContract()
            });

            // Assert
            basket.Items.Should().HaveCount(1);
        }
    }
}