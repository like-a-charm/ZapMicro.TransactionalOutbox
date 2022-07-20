using System;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.OutboxMessages
{
    public class OnOrderCreatedOutboxMessage: IOutboxMessage
    {
        public Guid OrderId { get; set; }
        public double OrderGrandTotal { get; set; }
    }
}