using System.Collections.Generic;

namespace Basket.Domain.Commands.GetBasketById
{
    public class CommandResultBase
    {
        public string ValidationState { get; set; }
        public List<string> Messages { get; set; }
        public string ReturnPath { get; set; }
    }
}