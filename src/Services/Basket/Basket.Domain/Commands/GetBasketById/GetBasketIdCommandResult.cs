using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.GetBasketById
{
    public class GetBasketIdCommandResult : CommandResultBase
    {
        public BasketContract BasketContract { get; set; }
    }
}