using Basket.Domain.DomainDtos;
using MediatR;

namespace Basket.Domain.Commands.GetBasketById
{
    public class GetBasketByIdCommand : IRequest<BasketDto>
    {
        public string Id { get; set; }
    }
}