using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.UpdateBasket
{
    [ExcludeFromCodeCoverage]
    public class UpdateBasketCommandResult : CommandResultBase
    {
        public BasketContract Basket { get; set; }
    }
}