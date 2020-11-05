using System;
using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Commands.DeleteBasket;
using MediatR;

namespace Basket.Domain.Commands.CheckoutBasket
{
    [ExcludeFromCodeCoverage]
    public class CheckoutBasketCommand : IRequest<CheckoutBasketCommandResult>
    {
        public string City { get; set; }

        public string Street { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string CardNumber { get; set; }

        public string CardHolderName { get; set; }

        public DateTime CardExpiration { get; set; }

        public string CardSecurityNumber { get; set; }

        public int CardTypeId { get; set; }

        public string Buyer { get; set; }
        public string BuyerId { get; set; }

        public Guid RequestId { get; set; }
    }
}