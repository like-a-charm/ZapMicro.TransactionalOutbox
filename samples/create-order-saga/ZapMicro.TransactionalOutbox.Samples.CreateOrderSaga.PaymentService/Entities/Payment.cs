using System;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Entities
{
    public class Payment: AggregateBase<Guid>
    {
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
    }
}