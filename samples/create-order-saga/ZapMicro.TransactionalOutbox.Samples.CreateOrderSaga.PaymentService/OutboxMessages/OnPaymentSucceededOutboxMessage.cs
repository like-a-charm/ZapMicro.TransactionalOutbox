using System;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.OutboxMessages
{
    public class OnPaymentSucceededOutboxMessage: IOutboxMessage
    {
        public Guid OrderId { get; set; }
    }
}