using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Entities
{
    /// <inheritdoc />
    /// <summary>
    ///  Source: https://github.com/VaughnVernon/IDDD_Samples_NET
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class EntityBase<TId> : IIdentity<TId>
    {
        protected EntityBase()
        {
        }

        protected EntityBase(TId id)
        {
            Id = id;
            Created = DateTimeOffset.Now.UtcDateTime;
        }

        public DateTime Created { get; private set; }

        public DateTime? Updated { get; private set; }

        // It must be public because of not moved from derived class. impedance mismatch :(
        [Key] public TId Id { get; set; }
    }
}