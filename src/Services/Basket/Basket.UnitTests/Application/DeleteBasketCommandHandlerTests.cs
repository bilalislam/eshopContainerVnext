using System.Threading;
using System.Threading.Tasks;
using Basket.Application.UseCases.DeleteBasket;
using Basket.Domain.Commands;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.RepositoryInterfaces;
using Moq;
using NUnit.Framework;

namespace Basket.UnitTests.Application
{
    [TestFixture]
    public class DeleteBasketCommandHandlerTests
    {
        private Mock<IBasketCommandRepository> _mockBasketCommandRepository;

        [SetUp]
        public void Init()
        {
            _mockBasketCommandRepository = new Mock<IBasketCommandRepository>();
        }

        [TearDown]
        public void Dispose()
        {
            _mockBasketCommandRepository.Reset();
        }

        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenRequestIsNull()
        {
            var command = new DeleteBasketCommandHandler(_mockBasketCommandRepository.Object);

            //Act
            var result = await command.Handle(It.IsAny<DeleteBasketCommand>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.NotAcceptable);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnInValid_WhenBasketDoesNotExists()
        {
            //Arrange
            var command = new DeleteBasketCommandHandler(_mockBasketCommandRepository.Object);
            DeleteBasketCommand deleteBasketCommand = new DeleteBasketCommand()
            {
                BuyerId = "1"
            };
            _mockBasketCommandRepository.Setup(x => x.DeleteAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(false);

            //Act
            var result = await command.Handle(deleteBasketCommand, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.DoesNotExist);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnValid_WhenBasketDeleted()
        {
            //Arrange
            var command = new DeleteBasketCommandHandler(_mockBasketCommandRepository.Object);
            DeleteBasketCommand deleteBasketCommand = new DeleteBasketCommand()
            {
                BuyerId = "1"
            };
            _mockBasketCommandRepository.Setup(x => x.DeleteAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(true);

            //Act
            var result = await command.Handle(deleteBasketCommand, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.Valid);
            Assert.IsTrue(result.Success);
        }
    }
}