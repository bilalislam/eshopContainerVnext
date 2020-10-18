using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Contracts;
using Basket.Domain.ValueObjects;

namespace Basket.Domain.Assemblers.Implementations
{
    public class MoneyAssembler : IMoneyAssembler
    {
        public MoneyContract ToContract(Money money)
        {
            return new MoneyContract()
            {
                Amount = money.Amount
            };
        }
    }
}