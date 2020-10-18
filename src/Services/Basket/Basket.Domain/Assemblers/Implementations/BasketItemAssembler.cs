using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Contracts;
using Basket.Domain.Entities;

namespace Basket.Domain.Assemblers.Implementations
{
    public class BasketItemAssembler : IBasketItemAssembler
    {
        private readonly IMoneyAssembler _moneyAssembler;

        public BasketItemAssembler(IMoneyAssembler moneyAssembler)
        {
            _moneyAssembler = moneyAssembler;
        }

        public BasketItemContract ToContract(BasketItem basketItem)
        {
            return new BasketItemContract()
            {
                ProductId = basketItem.ProductId,
                Quantity = basketItem.Quantity,
                PictureUrl = basketItem.PictureUrl,
                ProductName = basketItem.ProductName,
                UnitPrice = _moneyAssembler.ToContract(basketItem.UnitPrice),
                OldUnitPrice = _moneyAssembler.ToContract(basketItem.OldUnitPrice)
            };
        }
    }
}