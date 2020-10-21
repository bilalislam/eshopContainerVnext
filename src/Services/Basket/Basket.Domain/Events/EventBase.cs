using System;

namespace Basket.Domain.Events
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
        public Guid Id { get; private set; } = Guid.NewGuid();
        public int EventVersion { get; private set; } = 1;
        public DateTime OccurredOn { get; private set; } = DateTimeOffset.Now.UtcDateTime;
    }
}