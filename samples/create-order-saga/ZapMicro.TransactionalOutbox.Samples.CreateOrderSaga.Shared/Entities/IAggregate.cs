using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities
{
    public interface IAggregate<T>: IEntity<T>
    {
        DateTime CreatedAt { get; set; }
        DateTime LastUpdatedAt { get; set; }
    }
}