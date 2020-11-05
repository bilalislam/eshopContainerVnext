using System.Threading;
using System.Threading.Tasks;
using Basket.Application.UseCases.GetBasketById;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.RepositoryInterfaces;
using Basket.UnitTests.Helpers;
using Moq;
using NUnit.Framework;

namespace Basket.UnitTests.Application
{
    [TestFixture]
    public class GetBasketByIdCommandHandlerTests
    {
        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenRequestIsNull()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
            var command = new GetBasketByIdCommandHandler(mockBasketAssembler.Object, mockBasketQueryRepository.Object);

            //Act
            var result = await command.Handle(It.IsAny<GetBasketByIdCommand>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.NotAcceptable);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenBasketDoesNotExists()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
            mockBasketQueryRepository.Setup(x => x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((Domain.Aggregates.Basket) (null));

            var command = new GetBasketByIdCommandHandler(mockBasketAssembler.Object, mockBasketQueryRepository.Object);

            //Act
            var result = await command.Handle(new GetBasketByIdCommand()
            {
                Id = "1"
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.DoesNotExist);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.ReturnPath, "/basket");
        }

        [Test]
        public async Task Handle_ShouldReturnValid_WhenBasketExists()
        {
            //Arrange
            var mockBasketAssembler = new Mock<IBasketAssembler>();
            var mockBasketQueryRepository = new Mock<IBasketQueryRepository>();

            string basketId = "1";
            mockBasketQueryRepository.Setup(x => x.GetBasketAsync(basketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDataGenerator.CreateBasket);

            mockBasketAssembler.Setup(x => x.ToContract(FakeDataGenerator.CreateBasket()))
                .Returns(FakeDataGenerator.CreateBasketContract);

            var command = new GetBasketByIdCommandHandler(mockBasketAssembler.Object, mockBasketQueryRepository.Object);

            //Act
            var result = await command.Handle(new GetBasketByIdCommand()
            {
                Id = basketId
            }, It.IsAny<CancellationToken>());

            //Assert
            Assert.AreEqual(result.ValidateState, ValidationState.Valid);
            Assert.IsTrue(result.Success);
        }
    }
}