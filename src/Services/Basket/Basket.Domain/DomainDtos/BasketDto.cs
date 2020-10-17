using System.Collections.Generic;

namespace Basket.Domain.DomainDtos
{
    public class BasketDto
    {
        public string BuyerId { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
    }
}