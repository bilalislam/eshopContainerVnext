using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Contracts;
using MediatR;

namespace Basket.Domain.Commands.UpdateBasket
{
    [ExcludeFromCodeCoverage]
    public class UpdateBasketCommand : IRequest<UpdateBasketCommandResult>
    {
        public string BuyerId { get; set; }
        public List<BasketItemContract> Items { get; set; }
    }
}