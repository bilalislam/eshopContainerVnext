using MediatR;

namespace Basket.Domain.Commands.DeleteBasket
{
    public class DeleteBasketCommand : IRequest<DeleteBasketCommandResult>
    {
        public string BuyerId { get; set; }
    }
}