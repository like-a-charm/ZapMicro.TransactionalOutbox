using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}