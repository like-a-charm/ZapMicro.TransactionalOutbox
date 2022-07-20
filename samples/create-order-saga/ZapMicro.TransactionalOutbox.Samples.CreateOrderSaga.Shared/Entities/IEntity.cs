using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}