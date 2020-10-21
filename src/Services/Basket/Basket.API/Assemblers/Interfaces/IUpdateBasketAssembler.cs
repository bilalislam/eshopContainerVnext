using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;

namespace Basket.API.Assemblers.Interfaces
{
    public interface IUpdateBasketAssembler
    {
        UpdateBasketCommand ToCommand(BasketContract contract);
    }
}