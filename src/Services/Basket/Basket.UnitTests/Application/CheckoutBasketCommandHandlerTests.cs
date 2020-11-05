using System.Threading;
using System.Threading.Tasks;
using Basket.Application.IntegrationEvents.Events;
using Basket.Application.UseCases.CheckoutBasket;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.CheckoutBasket;
using Basket.Domain.RepositoryInterfaces;
using Basket.Infrastructure.Bus;
using Basket.UnitTests.Helpers;
using Moq;
using NUnit.Framework;

namespace Basket.UnitTests.Application
{
    [TestFixture]
    public class CheckoutBasketCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenBasketDoesNotExists()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
            var mockEventBus = new Mock<IEventBus>();

            var command = new CheckoutBasketCommandHandler(mockBasketQueryRepository.Object, mockBasketAssembler.Object,
                mockEventBus.Object);

            mockBasketQueryRepository.Setup(x =>
                    x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((Domain.Aggregates.Basket) null);

            //Act
            var result = await command.Handle(new CheckoutBasketCommand()
            {
                BuyerId = "1"
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.DoesNotExist);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenCanNotPublishedMessage()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
            var mockEventBus = new Mock<IEventBus>();

            var command = new CheckoutBasketCommandHandler(mockBasketQueryRepository.Object, mockBasketAssembler.Object,
                mockEventBus.Object);

            mockBasketQueryRepository.Setup(x =>
                    x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(FakeDataGenerator.CreateBasket);

            mockEventBus.Setup(x => x.PublishMessageAsync(
                    It.IsAny<UserCheckoutAcceptedIntegrationEvent>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(false);

            //Act
            var result = await command.Handle(new CheckoutBasketCommand()
            {
                BuyerId = "1"
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.UnProcessable);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnValid_WhenPublishedMessage()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
            var mockEventBus = new Mock<IEventBus>();

            var command = new CheckoutBasketCommandHandler(mockBasketQueryRepository.Object, mockBasketAssembler.Object,
                mockEventBus.Object);

            mockBasketQueryRepository.Setup(x =>
                    x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(FakeDataGenerator.CreateBasket);

            mockEventBus.Setup(x => x.PublishMessageAsync(
                    It.IsAny<UserCheckoutAcceptedIntegrationEvent>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            var result = await command.Handle(new CheckoutBasketCommand()
            {
                BuyerId = "1"
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.Valid);
            Assert.IsTrue(result.Success);
        }
    }
}