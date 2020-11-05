using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Basket.Domain.Assemblers.Interfaces;

namespace Basket.Domain.Assemblers.Implementations
{
    [ExcludeFromCodeCoverage]
    public class BasketAssembler : IBasketAssembler
    {
        private readonly IBasketItemAssembler _basketItemAssembler;

        public BasketAssembler(IBasketItemAssembler basketItemAssembler)
        {
            _basketItemAssembler = basketItemAssembler;
        }

        public Contracts.BasketContract ToContract(Aggregates.Basket basket)
        {
            return new Contracts.BasketContract()
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                BasketItems = basket.Items?.Select(x => _basketItemAssembler.ToContract(x))
            };
        }
    }
}