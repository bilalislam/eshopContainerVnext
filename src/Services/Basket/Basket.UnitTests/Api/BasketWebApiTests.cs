using System.Threading;
using System.Threading.Tasks;
using Basket.API.Assemblers.Interfaces;
using Basket.API.Controllers;
using Basket.API.Model;
using Basket.Domain.Commands;
using Basket.Domain.Commands.CheckoutBasket;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using Basket.UnitTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Basket.UnitTests.Api
{
    [TestFixture]
    public class BasketWebApiTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IBasketAssembler> _mockBasketAssembler;

        [SetUp]
        public void Init()
        {
            _mockMediator = new Mock<IMediator>();
            _mockBasketAssembler = new Mock<IBasketAssembler>();
        }

        [TearDown]
        public void Dispose()
        {
            _mockMediator.Reset();
            _mockBasketAssembler.Reset();
        }

        [Test]
        public async Task Get_customer_basket_success()
        {
            //Arrange
            var fakeCustomerId = "1";
            _mockMediator.Setup(x => x.Send(It.IsAny<GetBasketByIdCommand>(), CancellationToken.None))
                .ReturnsAsync(new GetBasketIdCommandResult()
                {
                    BasketContract = FakeDataGenerator.CreateBasketContract()
                });

            var basketController = new BasketController(_mockMediator.Object, _mockBasketAssembler.Object);

            //Act
            var actionResult = await basketController.GetBasketByIdAsync(fakeCustomerId, CancellationToken.None);

            //Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int) System.Net.HttpStatusCode.OK);
            Assert.AreEqual(
                (((ObjectResult) actionResult.Result).Value as GetBasketIdCommandResult).BasketContract.BuyerId,
                fakeCustomerId);
        }

        [Test]
        public async Task Post_customer_basket_success()
        {
            //Arrange
            var fakeCustomerId = "2";

            var updateBasketCommandResult = new UpdateBasketCommandResult()
            {
                Basket = FakeDataGenerator.CreateBasketContract("2")
            };

            _mockBasketAssembler.Setup(x => x.ToCommand(It.IsAny<BasketContract>()))
                .Returns(It.IsAny<UpdateBasketCommand>());

            _mockMediator.Setup(x => x.Send(It.IsAny<UpdateBasketCommand>(), CancellationToken.None))
                .ReturnsAsync(updateBasketCommandResult);

            var basketController = new BasketController(_mockMediator.Object, _mockBasketAssembler.Object);

            //Act
            var actionResult =
                await basketController.UpdateBasketAsync(It.IsAny<BasketContract>(),
                    CancellationToken.None);

            //Assert
            Assert.AreEqual((actionResult.Result as CreatedResult).StatusCode, (int) System.Net.HttpStatusCode.Created);
            Assert.AreEqual((((ObjectResult) actionResult.Result).Value as UpdateBasketCommandResult).Basket.BuyerId,
                fakeCustomerId);
        }


        [Test]
        public async Task Delete_customer_basket_success()
        {
            //Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<DeleteBasketCommand>(), CancellationToken.None));
            var basketController = new BasketController(_mockMediator.Object, _mockBasketAssembler.Object);

            //Act
            var actionResult =
                await basketController.DeleteBasketByIdAsync(It.IsAny<string>(),
                    CancellationToken.None);

            //Assert
            Assert.AreEqual((actionResult as NoContentResult).StatusCode, (int) System.Net.HttpStatusCode.NoContent);
        }

        [Test]
        public async Task Doing_Checkout_Without_Basket_Should_Return_Bad_Request()
        {
            //Arrange
            string fakeCustomerId = "2";
            _mockBasketAssembler.Setup(x => x.ToCommand(It.IsAny<BasketCheckout>()))
                .Returns(It.IsAny<CheckoutBasketCommand>());

            _mockMediator.Setup(x => x.Send(It.IsAny<CheckoutBasketCommand>(), CancellationToken.None))
                .ReturnsAsync(new CheckoutBasketCommandResult()
                {
                    ValidateState = ValidationState.DoesNotExist
                });

            var basketController = new BasketController(_mockMediator.Object, _mockBasketAssembler.Object);

            //Act
            var result = await basketController.CheckoutAsync(FakeDataGenerator.CreateBasketCheckout(fakeCustomerId),
                It.IsAny<string>(),
                CancellationToken.None);

            //Assert
            Assert.AreEqual((result as BadRequestResult).StatusCode, (int) System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Doing_Checkout_Wit_Basket_Should_Publish_UserCheckoutAccepted_Integration_Event()
        {
            //Arrange
            string fakeCustomerId = "2";
            _mockBasketAssembler.Setup(x => x.ToCommand(It.IsAny<BasketCheckout>()))
                .Returns(It.IsAny<CheckoutBasketCommand>());

            _mockMediator.Setup(x => x.Send(It.IsAny<CheckoutBasketCommand>(), CancellationToken.None))
                .ReturnsAsync(new CheckoutBasketCommandResult()
                {
                    ValidateState = ValidationState.Valid
                });

            var basketController = new BasketController(_mockMediator.Object, _mockBasketAssembler.Object);

            //Act
            var result = await basketController.CheckoutAsync(FakeDataGenerator.CreateBasketCheckout(fakeCustomerId),
                It.IsAny<string>(),
                CancellationToken.None);

            //Assert
            Assert.AreEqual((result as AcceptedResult).StatusCode, (int) System.Net.HttpStatusCode.Accepted);
        }
    }
}