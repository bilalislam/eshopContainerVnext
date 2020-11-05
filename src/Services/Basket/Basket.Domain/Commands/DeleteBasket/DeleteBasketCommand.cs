using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace Basket.Domain.Commands.DeleteBasket
{
    [ExcludeFromCodeCoverage]
    public class DeleteBasketCommand : IRequest<DeleteBasketCommandResult>
    {
        public string BuyerId { get; set; }
    }
}