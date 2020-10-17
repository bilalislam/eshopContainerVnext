using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.Aggregates.Entities;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.DomainDtos;
using MediatR;

namespace Basket.Application.UseCases.GetBasketById
{
    public class GetBasketByIdCommandHandler : IRequestHandler<GetBasketByIdCommand, BasketDto>
    {
        public Task<BasketDto> Handle(GetBasketByIdCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}