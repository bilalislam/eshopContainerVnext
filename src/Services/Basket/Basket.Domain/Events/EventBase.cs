using System;

namespace Basket.Domain.Aggregates
{
    /// <summary>
    ///  Supertype for all Event types
    /// </summary>
    public interface IEvent
    {
        Guid Id { get; }
        int EventVersion { get; }
        DateTime OccurredOn { get; }
    }

    public abstract class EventBase : IEvent
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public int EventVersion { get; protected set; } = 1;
        public DateTime OccurredOn { get; protected set; } = DateTimeOffset.Now.UtcDateTime;
    }
}