using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.ErrorCodes
{
    [ExcludeFromCodeCoverage]
    public static class DomainErrorCodes
    {
        public const string EDBasket1001 = "BasketCustomerIdCouldNotBeNull";
        public const string EDBasket1002 = "CurrencyNameCouldNotBeEmpty";
        public const string EDBasket1003 = "CurrencyCodeShoulNotBeEqualAndLessThenZero";
        public const string EDBasket1004 = "AmountCouldNotBeLessThenZero";
        public const string EDBasket1005 = "ProductNameCouldNotBeEmpty";
        public const string EDBasket1006 = "QuantityCouldNotBeEqualAndLessThenZero";
        public const string EDBasket1007 = "PictureUrlCouldNotBeEmpty";
    }
}