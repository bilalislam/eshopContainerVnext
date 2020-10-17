using System.Collections.Generic;
using Basket.Domain.Aggregates.Entities;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.Domain.Shared;

namespace Basket.Domain.Aggregates
{
    public class Basket : AggregateRootBase
    {
        public string BuyerId { get; private set; }

        public List<BasketItem> Items { get; private set; }

        /// <summary>
        /// Customer is an invariant.
        /// </summary>
        /// <param name="customerId"></param>
        private Basket(string customerId)
        {
            BuyerId = customerId;
            Items = new List<BasketItem>();
        }

        public static Basket Load(GetBasketByIdCommand getBasketByIdCommand)
        {
            Guard.That<DomainException>(string.IsNullOrEmpty(getBasketByIdCommand.Id),
                nameof(DomainErrorCodes.EDBasket1001), DomainErrorCodes.EDBasket1001);

            return new Basket(getBasketByIdCommand.Id);
        }
    }
}