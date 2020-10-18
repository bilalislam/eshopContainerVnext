using Basket.Domain.Contracts;

namespace Basket.Domain.Assemblers.Interfaces
{
    public interface IBasketAssembler
    {
        BasketContract ToContract(Aggregates.Basket basket);
    }
}