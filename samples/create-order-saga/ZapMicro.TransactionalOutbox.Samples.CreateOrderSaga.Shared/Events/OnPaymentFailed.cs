using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public class OnPaymentFailed : IEvent
    {
        public Guid OrderId { get; set; }
        public Guid Id { get; set; }
    }
}