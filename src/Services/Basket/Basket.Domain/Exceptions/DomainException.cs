using System;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DomainException : Exception
    {
        public string Code { get; private set; }

        public DomainException(string code, string message)
            : base(message)
        {
            this.Code = code;
        }
    }
}