using System;

namespace Basket.Domain.Aggregates.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }
        public DateTime Created => DateTimeOffset.Now.UtcDateTime;
        public DateTime Updated => DateTimeOffset.Now.UtcDateTime;

        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }

        protected EntityBase(Guid id)
        {
            Id = id;
        }
    }
}