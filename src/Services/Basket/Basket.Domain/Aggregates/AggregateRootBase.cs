using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Entities;
using Basket.Domain.Events;

namespace Basket.Domain.Aggregates
{
    public interface IAggregateRootWithId<TId>
    {
        List<IEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }

    [ExcludeFromCodeCoverage]
    public abstract class AggregateRootBase<TId> : EntityBase<TId>, IAggregateRootWithId<TId>
    {
        private readonly IDictionary<Type, Action<object>> _handlers = new ConcurrentDictionary<Type, Action<object>>();
        private List<IEvent> _uncommittedEvents = new List<IEvent>();

        protected AggregateRootBase() : this(default)
        {
        }

        protected AggregateRootBase(TId id) : base(id)
        {
        }

        public int Version { get; protected set; }

        protected void AddEvent(IEvent uncommittedEvent)
        {
            _uncommittedEvents ??= new List<IEvent>();
            Version++;
            _uncommittedEvents.Add(uncommittedEvent);
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public List<IEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }
    }
}