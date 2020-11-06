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
        private Mock<IBasketAssembler> _mockBasketAssembler;
        private Mock<IBasketQueryRepository> _mockBasketQueryRepository;


        [SetUp]
        public void Init()
        {
            _mockBasketAssembler = new Mock<IBasketAssembler>();
            _mockBasketQueryRepository = new Mock<IBasketQueryRepository>();
        }

        [TearDown]
        public void Dispose()
        {
            _mockBasketAssembler.Reset();
            _mockBasketQueryRepository.Reset();
        }

        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenRequestIsNull()
        {
            //Arrange
            var command =
                new GetBasketByIdCommandHandler(_mockBasketAssembler.Object, _mockBasketQueryRepository.Object);

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
            _mockBasketQueryRepository.Setup(x => x.GetBasketAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((Domain.Aggregates.Basket) (null));

            var command =
                new GetBasketByIdCommandHandler(_mockBasketAssembler.Object, _mockBasketQueryRepository.Object);

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
            string basketId = "1";
            _mockBasketQueryRepository.Setup(x => x.GetBasketAsync(basketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDataGenerator.CreateBasket);

            _mockBasketAssembler.Setup(x => x.ToContract(FakeDataGenerator.CreateBasket()))
                .Returns(FakeDataGenerator.CreateBasketContract());

            var command = new GetBasketByIdCommandHandler(_mockBasketAssembler.Object, _mockBasketQueryRepository.Object);

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