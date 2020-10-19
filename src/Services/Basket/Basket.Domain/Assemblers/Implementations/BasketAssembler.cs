using System.Linq;
using Basket.Domain.Assemblers.Interfaces;

namespace Basket.Domain.Assemblers.Implementations
{
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
                BuyerId = basket.BuyerId,
                BasketItems = basket.Items.Select(x => _basketItemAssembler.ToContract(x))
            };
        }
    }
}