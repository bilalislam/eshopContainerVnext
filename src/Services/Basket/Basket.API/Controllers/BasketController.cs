using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Basket.API.Assemblers.Interfaces;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Model;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.Services.Basket.API;
using Microsoft.eShopOnContainers.Services.Basket.API.Model;
using Microsoft.eShopOnContainers.Services.Basket.API.Services;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    // [Authorize]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketAssembler _basketAssembler;

        public BasketController(
            ILogger<BasketController> logger,
            IMediator mediator, IBasketAssembler basketAssembler)
        {
            _logger = logger;
            _mediator = mediator;
            _basketAssembler = basketAssembler;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetBasketIdCommandResult), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id,
            CancellationToken cancellationToken)
        {
            var getBasketIdCommandResult = await _mediator.Send(new GetBasketByIdCommand()
            {
                Id = id
            }, cancellationToken);
            return Ok(getBasketIdCommandResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateBasketCommandResult), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] BasketContract value,
            CancellationToken cancellationToken)
        {
            var command = _basketAssembler.ToCommand(value);
            var updateBasketCommandResult = await _mediator.Send(command, cancellationToken);
            return Created("api/v1/Basket", updateBasketCommandResult);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasketByIdAsync(string id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteBasketCommand()
            {
                BuyerId = id,
            }, cancellationToken);
            return NoContent();
        }

        // [Route("checkout")]
        // [HttpPost]
        // [ProducesResponseType((int) HttpStatusCode.Accepted)]
        // [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        // public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout,
        //     [FromHeader(Name = "x-requestid")] string requestId)
        // {
        //     //var userId = _identityService.GetUserIdentity();
        //     
        //     basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        //         ? guid
        //         : basketCheckout.RequestId;
        //     
        //     //var basket = await _repository.GetBasketAsync(userId);
        //     
        //     // if (basket == null)
        //     // {
        //     //     return BadRequest();
        //     // }
        //     
        //     //var userName = this.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;
        //
        //     _mediator.Send(_basketAssembler);
        //     
        //     var eventMessage = new UserCheckoutAcceptedIntegrationEvent(userId, userName, basketCheckout.City,
        //         basketCheckout.Street,
        //         basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber,
        //         basketCheckout.CardHolderName,
        //         basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId,
        //         basketCheckout.Buyer, basketCheckout.RequestId, basket);
        //     
        //     // Once basket is checkout, sends an integration event to
        //     // ordering.api to convert basket to order and proceeds with
        //     // order creation process
        //     try
        //     {
        //         //_eventBus.Publish(eventMessage);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}",
        //             eventMessage.Id, Program.AppName);
        //     
        //         throw;
        //     }
        //
        //     return Accepted();
        // }

        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout,
            [FromHeader(Name = "x-requestid")] string requestId, CancellationToken cancellationToken)
        {
            basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
                ? guid
                : basketCheckout.RequestId;

            var result = await _mediator.Send(_basketAssembler.ToCommand(basketCheckout), cancellationToken);

            if (result == null)
                return BadRequest();

            return Accepted();
        }
    }
}