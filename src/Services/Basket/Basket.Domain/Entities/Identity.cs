namespace Basket.Domain.Entities
{
    /// <summary>
    /// Supertype for all Identity types with generic Id
    /// </summary>
    public interface IIdentity<TId>
    {
        TId Id { get; }
    }
}