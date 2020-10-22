using Basket.API.Model;
using Basket.Domain.Commands.CheckoutBasket;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.Contracts;

namespace Basket.API.Assemblers.Interfaces
{
    public interface IBasketAssembler
    {
        UpdateBasketCommand ToCommand(BasketContract contract);
        CheckoutBasketCommand ToCommand(BasketCheckout contract);
    }
}