using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.GetBasketById;
using Basket.Domain.Contracts;
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
            if (request == null)
            {
                return new GetBasketIdCommandResult()
                {
                    ValidateState = ValidationState.NotAcceptable,
                    ReturnPath = "/basket"
                };
            }

            var basket = await _basketQueryRepository.GetBasketAsync(request.Id, cancellationToken);
            if (basket == null)
            {
                return new GetBasketIdCommandResult()
                {
                    ValidateState = ValidationState.DoesNotExist,
                    Messages = new[]
                    {
                        new MessageContractDto
                        {
                            Title = $"Basket does not exists by {request.Id}"
                        }
                    },
                    ReturnPath = "/basket"
                };
            }

            return new GetBasketIdCommandResult()
            {
                BasketContract = _basketAssembler.ToContract(basket),
                ValidateState = ValidationState.Valid
            };
        }
    }
}