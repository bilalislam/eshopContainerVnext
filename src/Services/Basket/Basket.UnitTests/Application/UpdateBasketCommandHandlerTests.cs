using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Basket.Application.UseCases.UpdateBasket;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using Basket.Domain.RepositoryInterfaces;
using Basket.UnitTests.Helpers;
using Moq;
using NUnit.Framework;

namespace Basket.UnitTests.Application
{
    [TestFixture]
    public class UpdateBasketCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenRequestIsNull()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketCommandRepository = new Mock<IBasketCommandRepository>();
            var command =
                new UpdateBasketCommandHandler(mockBasketAssembler.Object, mockBasketCommandRepository.Object);

            //Act
            var result = await command.Handle(It.IsAny<UpdateBasketCommand>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.NotAcceptable);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnInValid_WhenBasketDoesNotExists()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketCommandRepository = new Mock<IBasketCommandRepository>();
            var command =
                new UpdateBasketCommandHandler(mockBasketAssembler.Object, mockBasketCommandRepository.Object);

            mockBasketCommandRepository.Setup(x =>
                    x.SaveAsync(It.IsAny<Domain.Aggregates.Basket>(), CancellationToken.None))
                .ReturnsAsync(false);

            //Act
            var result = await command.Handle(new UpdateBasketCommand()
            {
                BuyerId = "1",
                Items = new List<BasketItemContract>()
                {
                    FakeDataGenerator.CreateBasketItemContract()
                }
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.DoesNotExist);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnValid_WhenBasketUpdated()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketCommandRepository = new Mock<IBasketCommandRepository>();
            var command =
                new UpdateBasketCommandHandler(mockBasketAssembler.Object, mockBasketCommandRepository.Object);

            mockBasketCommandRepository.Setup(x =>
                    x.SaveAsync(It.IsAny<Domain.Aggregates.Basket>(), CancellationToken.None))
                .ReturnsAsync(true);

            //Act
            var result = await command.Handle(new UpdateBasketCommand()
            {
                BuyerId = "1",
                Items = new List<BasketItemContract>()
                {
                    FakeDataGenerator.CreateBasketItemContract()
                }
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.Valid);
            Assert.IsTrue(result.Success);
        }
    }
}