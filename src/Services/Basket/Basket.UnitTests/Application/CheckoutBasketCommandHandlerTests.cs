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
        private Mock<IBasketAssembler> _mockBasketAssembler;
        private Mock<IBasketQueryRepository> _mockBasketQueryRepository;
        private Mock<IEventBus> _mockEventBus;


        [SetUp]
        public void Init()
        {
            _mockBasketAssembler = new Mock<IBasketAssembler>();
            _mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
            _mockEventBus = new Mock<IEventBus>();
        }

        [TearDown]
        public void Dispose()
        {
            _mockBasketAssembler.Reset();
            _mockBasketQueryRepository.Reset();
            _mockEventBus.Reset();
        }

        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenBasketDoesNotExists()
        {
            var command = new CheckoutBasketCommandHandler(_mockBasketQueryRepository.Object,
                _mockBasketAssembler.Object,
                _mockEventBus.Object);

            _mockBasketQueryRepository.Setup(x =>
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
            var command = new CheckoutBasketCommandHandler(_mockBasketQueryRepository.Object,
                _mockBasketAssembler.Object,
                _mockEventBus.Object);

            _mockBasketQueryRepository.Setup(x =>
                    x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(FakeDataGenerator.CreateBasket);

            _mockEventBus.Setup(x => x.PublishMessageAsync(
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
            var command = new CheckoutBasketCommandHandler(_mockBasketQueryRepository.Object,
                _mockBasketAssembler.Object,
                _mockEventBus.Object);

            _mockBasketQueryRepository.Setup(x =>
                    x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(FakeDataGenerator.CreateBasket);

            _mockEventBus.Setup(x => x.PublishMessageAsync(
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