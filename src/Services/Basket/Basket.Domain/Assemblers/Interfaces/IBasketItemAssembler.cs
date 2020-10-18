using Basket.Domain.Contracts;
using Basket.Domain.Entities;

namespace Basket.Domain.Assemblers.Interfaces
{
    public interface IBasketItemAssembler
    {
        BasketItemContract ToContract(BasketItem basketItem);
    }
}