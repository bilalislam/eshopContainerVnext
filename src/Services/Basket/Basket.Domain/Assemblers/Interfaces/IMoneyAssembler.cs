using Basket.Domain.Contracts;
using Basket.Domain.ValueObjects;

namespace Basket.Domain.Assemblers.Interfaces
{
    public interface IMoneyAssembler
    {
        MoneyContract ToContract(Money money);
    }
}