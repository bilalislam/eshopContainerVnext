using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;
using Basket.Domain.Entities;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Events;
using Basket.Domain.Exceptions;
using Basket.Domain.Shared;

namespace Basket.Domain.Aggregates
{
    /// <summary>
    /// All normal basket case which can be in real life will include next time step by step ...
    /// https://github.com/vietnam-devs/coolstore-microservices/blob/develop/src/microservices/shopping-cart-service/VND.CoolStore.ShoppingCart/Domain/Cart/Cart.cs
    /// </summary>
    public class Basket : AggregateRootBase<Guid>
    {
        public string BuyerId { get; private set; }

        public List<BasketItem> Items { get; private set; }


        /// <summary>
        /// Business Invariant is just a buyer but items are defining that's why impedance mismatch ! 
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="items"></param>
        public Basket(string buyerId, List<BasketItem> items = null) : base(Guid.NewGuid())
        {
            BuyerId = buyerId;
            Items = items;
            AddEvent(new BasketCreated(buyerId));
        }


        /// <summary>
        /// Domain Validation : https://medium.com/@cem.basaranoglu/we-dont-need-no-command-validation-1226c40c3df8
        /// </summary>
        /// <param name="updateBasketCommand"></param>
        /// <returns></returns>
        public static Basket Load(UpdateBasketCommand updateBasketCommand)
        {
            Guard.That<DomainException>(updateBasketCommand == null,
                nameof(DomainErrorCodes.EDBasket1000), DomainErrorCodes.EDBasket1000);

            Guard.That<DomainException>(string.IsNullOrEmpty(updateBasketCommand.BuyerId),
                nameof(DomainErrorCodes.EDBasket1001), DomainErrorCodes.EDBasket1001);

            return new Basket(updateBasketCommand.BuyerId);
        }

        public Basket AddCartItem(IEnumerable<BasketItemContract> basketItemContracts)
        {
            Guard.That<DomainException>(basketItemContracts == null || !basketItemContracts.Any(),
                nameof(DomainErrorCodes.EDBasket1008),
                DomainErrorCodes.EDBasket1008);

            Items = new List<BasketItem>();
            foreach (var itemContract in basketItemContracts)
            {
                var basketItem = BasketItem.Load(itemContract);
                Items.Add(basketItem);
                AddEvent(new BasketItemAdded(BuyerId, basketItem));
            }

            return this;
        }
    }
}