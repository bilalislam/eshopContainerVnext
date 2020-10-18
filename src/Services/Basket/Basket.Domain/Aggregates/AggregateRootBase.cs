using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Basket.Domain.Entities;
using Basket.Domain.Events;

namespace Basket.Domain.Aggregates
{
    public interface IAggregateRoot<TId>
    {
        List<IEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();

        IAggregateRoot<TId> AddEvent(IEvent uncommittedEvent);
    }

    [ExcludeFromCodeCoverage]
    public abstract class AggregateRootBase<TId> : EntityBase<TId>, IAggregateRoot<TId>
    {
        private readonly IDictionary<Type, Action<object>> _handlers = new ConcurrentDictionary<Type, Action<object>>();
        private readonly List<IEvent> _uncommittedEvents = new List<IEvent>();

        protected AggregateRootBase() : this(default)
        {
        }

        protected AggregateRootBase(TId id) : base(id)
        {
        }

        public int Version { get; protected set; }

        public IAggregateRoot<TId> AddEvent(IEvent uncommittedEvent)
        {
            _uncommittedEvents.Add(uncommittedEvent);
            ApplyEvent(uncommittedEvent);
            return this;
        }

        public IAggregateRoot<TId> ApplyEvent(IEvent payload)
        {
            if (!_handlers.ContainsKey(payload.GetType()))
                return this;
            _handlers[payload.GetType()]?.Invoke(payload);
            Version++;
            return this;
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