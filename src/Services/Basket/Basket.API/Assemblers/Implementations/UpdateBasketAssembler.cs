using System.Linq;
using Basket.API.Assemblers.Interfaces;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;

namespace Basket.API.Assemblers.Implementations
{
    public class UpdateBasketAssembler : IUpdateBasketAssembler
    {
        public UpdateBasketCommand ToCommand(BasketContract contract)
        {
            return new UpdateBasketCommand()
            {
                BuyerId = contract.BuyerId,
                Items = contract.BasketItems.ToList()
            };
        }
    }
}