using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.UpdateBasket
{
    public class UpdateBasketCommandResult
    {
        public BasketContract Basket { get; set; }
    }
}