using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class MoneyContract
    {
        public decimal Amount { get; set; }
    }
}