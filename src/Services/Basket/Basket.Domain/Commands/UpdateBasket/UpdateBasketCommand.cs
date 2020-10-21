using System.Collections.Generic;
using Basket.Domain.Contracts;
using MediatR;

namespace Basket.Domain.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest<UpdateBasketCommandResult>
    {
        public string BuyerId { get; set; }
        public List<BasketItemContract> Items { get; set; }
    }
}