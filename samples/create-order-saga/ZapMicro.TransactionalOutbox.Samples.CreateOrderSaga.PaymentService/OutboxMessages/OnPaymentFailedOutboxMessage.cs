using System;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.OutboxMessages
{
    public class OnPaymentFailedOutboxMessage: IOutboxMessage
    {
        public Guid OrderId { get; set; }
    }
}