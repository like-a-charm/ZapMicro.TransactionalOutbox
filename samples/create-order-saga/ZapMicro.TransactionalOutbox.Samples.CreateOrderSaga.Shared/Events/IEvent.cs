using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public interface IEvent
    {
        public Guid Id { get; set; }
    }
}