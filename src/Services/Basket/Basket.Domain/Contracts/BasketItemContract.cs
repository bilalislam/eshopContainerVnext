using System;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class BasketItemContract
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public MoneyContract UnitPrice { get; set; }
        public MoneyContract OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}