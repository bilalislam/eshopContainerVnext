using System;
using Basket.Domain.Contracts;
using Basket.Domain.Events;

namespace Basket.Application.IntegrationEvents.Events
{
    public class UserCheckoutAcceptedIntegrationEvent : EventBase
    {
        public string UserId { get; }

        public string UserName { get; }

        public string City { get; }

        public string Street { get; }

        public string State { get; }

        public string Country { get; }

        public string ZipCode { get; }

        public string CardNumber { get; }

        public string CardHolderName { get; }

        public DateTime CardExpiration { get; }

        public string CardSecurityNumber { get; }

        public int CardTypeId { get; }

        public string Buyer { get; }

        public Guid RequestId { get; }

        public BasketContract Basket { get; }


        public UserCheckoutAcceptedIntegrationEvent(string userId, string userName, string city, string street,
            string state, string country, string zipCode, string cardNumber, string cardHolderName,
            DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, string buyer, Guid requestId,
            BasketContract basket)
        {
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            Buyer = buyer;
            Basket = basket;
            RequestId = requestId;
        }
    }
}