using System;
using System.Threading;
using System.Threading.Tasks;
using Basket.Application.IntegrationEvents.Events;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.RepositoryInterfaces;
using MediatR;

namespace Basket.Application.UseCases.CheckoutBasket
{
    public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, CheckoutBasketCommandResult>
    {
        private readonly IBasketQueryRepository _basketQueryRepository;
        private readonly IBasketAssembler _basketAssembler;

        public CheckoutBasketCommandHandler(IBasketQueryRepository basketQueryRepository,
            IBasketAssembler basketAssembler)
        {
            _basketQueryRepository = basketQueryRepository;
            _basketAssembler = basketAssembler;
        }

        /// <summary>
        /// Once basket is checkout, sends an integration event to
        /// ordering.api to convert basket to order and proceeds with
        /// order creation process
        /// Customer  Shipping and Payment Information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<CheckoutBasketCommandResult> Handle(CheckoutBasketCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketQueryRepository.GetBasketAsync(request.BuyerId, cancellationToken);

            if (basket == null)
                return null;

            var eventMessage = new UserCheckoutAcceptedIntegrationEvent(
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


            return new CheckoutBasketCommandResult();
        }
    }
}