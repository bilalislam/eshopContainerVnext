using System;
using System.ComponentModel.DataAnnotations;

namespace Basket.Domain.Entities
{
    /// <inheritdoc />
    /// <summary>
    ///  Source: https://github.com/VaughnVernon/IDDD_Samples_NET
    /// </summary>
    public abstract class EntityBase<TId> : IIdentity<TId>
    {
        protected EntityBase(TId id)
        {
            Id = id;
            Created = DateTimeOffset.Now.UtcDateTime;
        }

        public DateTime Created { get; protected set; }

        public DateTime? Updated { get; protected set; }

        [Key] 
        public TId Id { get; protected set; }
    }
}