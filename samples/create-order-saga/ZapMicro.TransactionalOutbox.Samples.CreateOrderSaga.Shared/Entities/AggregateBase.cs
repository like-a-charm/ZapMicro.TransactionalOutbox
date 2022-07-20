using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities
{
    public abstract class AggregateBase<T> : EntityBase<T>, IAggregate<T>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}