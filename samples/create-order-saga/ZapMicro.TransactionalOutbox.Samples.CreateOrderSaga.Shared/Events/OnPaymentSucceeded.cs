using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public class OnPaymentSucceeded: EventBase
    {
        public Guid OrderId { get; set; }
    }
}