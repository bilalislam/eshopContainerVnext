using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Basket.Domain.ErrorCodes;
using Basket.Domain.Exceptions;
using Basket.Domain.Shared;

namespace Basket.Domain.ValueObjects
{
    [ExcludeFromCodeCoverage]
    public class Currency : ValueObjectBase
    {
        //Impedance Mismathc :(
        public Currency(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public static Currency Load(string name, int code)
        {
            Guard.That<DomainException>(string.IsNullOrEmpty(name),
                nameof(DomainErrorCodes.EDBasket1002), DomainErrorCodes.EDBasket1002);

            Guard.That<DomainException>(code <= 0, nameof(DomainErrorCodes.EDBasket1003),
                DomainErrorCodes.EDBasket1003);

            return new Currency(name, code);
        }

        public string Name { get; private set; }
        public int Code { get; private set; }

        [ExcludeFromCodeCoverage]
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Code;
        }
    }
}