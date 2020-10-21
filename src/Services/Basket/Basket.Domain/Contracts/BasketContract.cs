using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class BasketContract
    {
        public Guid Id { get; set; }
        public string BuyerId { get; set; }
        public IEnumerable<BasketItemContract> BasketItems { get; set; }
    }
}