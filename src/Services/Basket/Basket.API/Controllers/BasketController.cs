using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Basket.API.Assemblers.Interfaces;
using Basket.API.Model;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.Services.Basket.API.Model;

namespace Basket.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    // [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBasketAssembler _basketAssembler;

        public BasketController(
            IMediator mediator,
            IBasketAssembler basketAssembler)
        {
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