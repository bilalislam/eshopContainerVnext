using MediatR;

namespace Basket.Domain.Commands.GetBasketById
{
    /// <summary>
    /// Command must be immutable
    /// </summary>
    public class GetBasketByIdCommand : IRequest<GetBasketIdCommandResult>
    {
        public string Id { get; set; }
    }
}