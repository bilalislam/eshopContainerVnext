using System.Collections.Generic;
using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.GetBasketById
{
    public class CommandResultBase
    {
        public ValidationState ValidateState { get; set; }
        public IEnumerable<MessageContractDto> Messages { get; set; }
        public string ReturnPath { get; set; }

        public bool Success => ValidateState == ValidationState.Valid;
    }
}