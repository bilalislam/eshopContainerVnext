using System.Threading;
using System.Threading.Tasks;
using Basket.Application.IntegrationEvents.Events;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.CheckoutBasket;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.RepositoryInterfaces;
using Basket.Infrastructure.Bus;
using MediatR;

namespace Basket.Application.UseCases.CheckoutBasket
{
    public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, CheckoutBasketCommandResult>
    {
        private readonly IBasketQueryRepository _basketQueryRepository;
        private readonly IBasketAssembler _basketAssembler;
        private readonly IEventBus _eventBus;

        public CheckoutBasketCommandHandler(IBasketQueryRepository basketQueryRepository,
            IBasketAssembler basketAssembler, IEventBus eventBus)
        {
            _basketQueryRepository = basketQueryRepository;
            _basketAssembler = basketAssembler;
            _eventBus = eventBus;
        }

        /// <summary>
        /// Once basket is checkout, sends an integration event to
        /// ordering.api to convert basket to order and proceeds with
        /// order creation process
        /// The Shipping and Payment Information  Of Customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CheckoutBasketCommandResult> Handle(CheckoutBasketCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketQueryRepository.GetBasketAsync(request.BuyerId, cancellationToken);
            if (basket == null)
            {
                return new CheckoutBasketCommandResult()
                {
                    ValidateState = ValidationState.DoesNotExist,
                    ReturnPath = "/basket"
                };
            }

            var userCheckoutAcceptedIntegrationEvent = new UserCheckoutAcceptedIntegrationEvent(
                request.BuyerId,
                request.Buyer,
                request.City,
                request.Street,
                request.State,
                request.Country,
                request.ZipCode,
                request.CardNumber,
                request.CardHolderName,
                request.CardExpiration,
                request.CardSecurityNumber,
                request.CardTypeId,
                request.Buyer,
                request.RequestId,
                _basketAssembler.ToContract(basket));


            await _eventBus.PublishMessageAsync(userCheckoutAcceptedIntegrationEvent, "eshopContainers", "topic",
                nameof(userCheckoutAcceptedIntegrationEvent));

            return new CheckoutBasketCommandResult()
            {
                ValidateState = ValidationState.Valid
            };
        }
    }
}