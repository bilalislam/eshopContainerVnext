using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace Basket.Domain.Commands.GetBasketById
{
    /// <summary>
    /// Command must be immutable
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GetBasketByIdCommand : IRequest<GetBasketIdCommandResult>
    {
        public string Id { get; set; }
    }
}