using System.Threading;
using System.Threading.Tasks;
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
            await _basketCommandRepository.DeleteAsync(request.BuyerId, cancellationToken);
            return new DeleteBasketCommandResult();
        }
    }
}