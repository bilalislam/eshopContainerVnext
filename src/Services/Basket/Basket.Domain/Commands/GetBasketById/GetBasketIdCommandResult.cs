using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.GetBasketById
{
    [ExcludeFromCodeCoverage]
    public class GetBasketIdCommandResult : CommandResultBase
    {
        public BasketContract BasketContract { get; set; }
    }
}