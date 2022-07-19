using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public class OnPaymentSucceded: EventBase
    {
        public Guid OrderId { get; set; }
    }
}