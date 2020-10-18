﻿using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Model;
using Basket.Domain.Commands.GetBasketById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
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
        private readonly IBasketRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BasketController> _logger;
        private readonly IMediator _mediator;

        public BasketController(
            ILogger<BasketController> logger,
            IBasketRepository repository,
            IIdentityService identityService,
            IEventBus eventBus, IMediator mediator)
        {
            _logger = logger;
            _repository = repository;
            _identityService = identityService;
            _eventBus = eventBus;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetBasketIdCommandResult), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id,
            CancellationToken cancellationToken)
        {
            var getBasketIdCommandResult = await _mediator.Send(new GetBasketByIdCommand(), cancellationToken);
            return Ok(getBasketIdCommandResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
        {
            return Ok(await _repository.UpdateBasketAsync(value));
        }

        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout,
            [FromHeader(Name = "x-requestid")] string requestId)
        {
            var userId = _identityService.GetUserIdentity();

            basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
                ? guid
                : basketCheckout.RequestId;

            var basket = await _repository.GetBasketAsync(userId);

            if (basket == null)
            {
                return BadRequest();
            }

            var userName = this.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;

            var eventMessage = new UserCheckoutAcceptedIntegrationEvent(userId, userName, basketCheckout.City,
                basketCheckout.Street,
                basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber,
                basketCheckout.CardHolderName,
                basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId,
                basketCheckout.Buyer, basketCheckout.RequestId, basket);

            // Once basket is checkout, sends an integration event to
            // ordering.api to convert basket to order and proceeds with
            // order creation process
            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}",
                    eventMessage.Id, Program.AppName);

                throw;
            }

            return Accepted();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task DeleteBasketByIdAsync(string id)
        {
            await _repository.DeleteBasketAsync(id);
        }
    }
}