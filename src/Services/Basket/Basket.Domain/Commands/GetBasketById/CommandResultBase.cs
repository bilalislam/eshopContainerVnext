using System.Collections.Generic;
using System.Linq;
using Basket.Domain.Contracts;

namespace Basket.Domain.Commands.GetBasketById
{
    public class CommandResultBase
    {
        public virtual ValidationState ValidateState { get; set; }
        public virtual IEnumerable<MessageContractDto> Messages { get; set; }
        public string ReturnPath { get; set; }

        public bool Success => ValidateState == ValidationState.Valid;
    }
}