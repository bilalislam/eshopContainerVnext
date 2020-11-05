using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.Assemblers.Interfaces;
using Basket.Domain.Commands;
using Basket.Domain.Commands.UpdateBasket;
using Basket.Domain.RepositoryInterfaces;
using MediatR;

namespace Basket.Application.UseCases.UpdateBasket
{
    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, UpdateBasketCommandResult>
    {
        private readonly IBasketAssembler _basketAssembler;
        private readonly IBasketCommandRepository _basketCommandRepository;

        public UpdateBasketCommandHandler(IBasketAssembler basketAssembler,
            IBasketCommandRepository basketCommandRepository)
        {
            _basketCommandRepository = basketCommandRepository;
            _basketAssembler = basketAssembler;
        }

        /// <summary>
        /// this code block might be at Domain service or just in domain
        /// but at  now application layer now as a orchestrator domain code
        /// Application could not contains domain logic but can call it
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UpdateBasketCommandResult> Handle(UpdateBasketCommand request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new UpdateBasketCommandResult()
                {
                    ValidateState = ValidationState.NotAcceptable,
                    ReturnPath = "/basket"
                };
            }

            var basket = Domain.Aggregates.Basket
                .Load(request)
                .AddCartItem(request.Items);

            var isUpdated = await _basketCommandRepository.SaveAsync(basket, cancellationToken);
            if (!isUpdated)
            {
                return new UpdateBasketCommandResult()
                {
                    ValidateState = ValidationState.DoesNotExist,
                    ReturnPath = "/basket"
                };
            }

            return new UpdateBasketCommandResult()
            {
                Basket = _basketAssembler.ToContract(basket),
                ValidateState = ValidationState.Valid
            };
        }
    }
}