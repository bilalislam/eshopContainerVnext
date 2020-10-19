using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Entities;
using Basket.Domain.RepositoryInterfaces;
using MediatR;

namespace Basket.Application.UseCases.GetBasketById
{
    public class GetBasketByIdCommandHandler : IRequestHandler<GetBasketByIdCommand, GetBasketIdCommandResult>
    {
        private readonly IBasketAssembler _basketAssembler;
        private readonly IBasketQueryRepository _basketQueryRepository;

        public GetBasketByIdCommandHandler(IBasketAssembler basketAssembler,
            IBasketQueryRepository basketQueryRepository)
        {
            _basketAssembler = basketAssembler;
            _basketQueryRepository = basketQueryRepository;
        }

        public async Task<GetBasketIdCommandResult> Handle(GetBasketByIdCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketQueryRepository.GetBasketAsync(request.Id);
            return new GetBasketIdCommandResult()
            {
                BasketContract = _basketAssembler.ToContract(basket)
            };
        }
    }
}