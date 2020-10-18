using System;
using System.Collections.Generic;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Entities;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.Domain.Shared;

namespace Basket.Domain.Aggregates
{
    public class Basket : AggregateRootBase<Guid>
    {
        public string BuyerId { get; private set; }

        public List<BasketItem> Items { get; private set; }

        public static Basket Load(GetBasketByIdCommand getBasketByIdCommand)
        {
            Guard.That<DomainException>(string.IsNullOrEmpty(getBasketByIdCommand.Id),
                nameof(DomainErrorCodes.EDBasket1001), DomainErrorCodes.EDBasket1001);

            var basket = new Basket()
            {
                BuyerId = getBasketByIdCommand.Id,
                Items = new List<BasketItem>()
            };
            return basket;
        }
    }
}