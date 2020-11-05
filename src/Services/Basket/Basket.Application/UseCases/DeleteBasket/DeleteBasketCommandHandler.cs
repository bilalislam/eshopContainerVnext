using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Basket.Domain.Commands;
using Basket.Domain.Commands.DeleteBasket;
using Basket.Domain.RepositoryInterfaces;
using MediatR;

namespace Basket.Application.UseCases.DeleteBasket
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, DeleteBasketCommandResult>
    {
        private readonly IBasketCommandRepository _basketCommandRepository;

        public DeleteBasketCommandHandler(IBasketCommandRepository basketCommandRepository)
        {
            _basketCommandRepository = basketCommandRepository;
        }

        public async Task<DeleteBasketCommandResult> Handle(DeleteBasketCommand request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new DeleteBasketCommandResult()
                {
                    ValidateState = ValidationState.NotAcceptable,
                    ReturnPath = "/basket"
                };
            }

            var isDeleted = await _basketCommandRepository.DeleteAsync(request.BuyerId, cancellationToken);
            if (!isDeleted)
            {
                return new DeleteBasketCommandResult()
                {
                    ValidateState = ValidationState.DoesNotExist,
                    ReturnPath = "/basket"
                };
            }

            return new DeleteBasketCommandResult()
            {
                ValidateState = ValidationState.Valid
            };
        }
    }
}