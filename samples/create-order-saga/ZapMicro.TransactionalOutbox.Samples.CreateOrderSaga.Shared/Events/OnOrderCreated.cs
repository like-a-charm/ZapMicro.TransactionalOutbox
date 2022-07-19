using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public class OnOrderCreated: EventBase
    {
        public Guid OrderId { get; set; }
        public decimal OrderGrandTotal { get; set; }
    }
}