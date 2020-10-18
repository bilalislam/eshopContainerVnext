using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.GetBasketById
{
    public class GetBasketIdCommandResult
    {
        public BasketContract BasketContract { get; set; }
    }
}