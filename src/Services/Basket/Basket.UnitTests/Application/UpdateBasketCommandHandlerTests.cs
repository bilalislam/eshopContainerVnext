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
        private Mock<IBasketAssembler> _mockBasketAssembler;
        private Mock<IBasketCommandRepository> _mockBasketCommandRepository;


        [SetUp]
        public void Init()
        {
            _mockBasketAssembler = new Mock<IBasketAssembler>();
            _mockBasketCommandRepository = new Mock<IBasketCommandRepository>();
        }

        [TearDown]
        public void Dispose()
        {
            _mockBasketAssembler.Reset();
            _mockBasketCommandRepository.Reset();
        }

        [Test]
        public async Task Handle_ShouldReturnInvalid_WhenRequestIsNull()
        {
            //Arrange
            var command =
                new UpdateBasketCommandHandler(_mockBasketAssembler.Object, _mockBasketCommandRepository.Object);

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
            var command =
                new UpdateBasketCommandHandler(_mockBasketAssembler.Object, _mockBasketCommandRepository.Object);

            _mockBasketCommandRepository.Setup(x =>
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
            var command =
                new UpdateBasketCommandHandler(_mockBasketAssembler.Object, _mockBasketCommandRepository.Object);

            _mockBasketCommandRepository.Setup(x =>
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