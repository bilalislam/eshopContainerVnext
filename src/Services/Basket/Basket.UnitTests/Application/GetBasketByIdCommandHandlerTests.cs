using System.Threading;
using System.Threading.Tasks;
using Basket.Application.UseCases.GetBasketById;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.RepositoryInterfaces;
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
    }
}