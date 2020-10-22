using System;
using System.Linq;
using Basket.API.Assemblers.Interfaces;
using Basket.API.Model;
using Basket.Domain.Commands.CheckoutBasket;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;

namespace Basket.API.Assemblers.Implementations
{
    public class BasketAssembler : IBasketAssembler
    {
        public UpdateBasketCommand ToCommand(BasketContract contract)
        {
            return new UpdateBasketCommand()
            {
                BuyerId = contract.BuyerId,
                Items = contract.BasketItems.ToList()
            };
        }

        public CheckoutBasketCommand ToCommand(BasketCheckout contract)
        {
            return new CheckoutBasketCommand()
            {
                City = contract.City,
                Buyer = contract.Buyer,
                BuyerId = contract.BuyerId,
                Country = contract.Country,
                State = contract.State,
                Street = contract.Street,
                CardExpiration = contract.CardExpiration,
                CardNumber = contract.CardNumber,
                CardHolderName = contract.CardHolderName,
                CardSecurityNumber = contract.CardSecurityNumber,
                CardTypeId = contract.CardTypeId,
                ZipCode = contract.ZipCode,
                RequestId = contract.RequestId
            };
        }
    }
}