using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Handlers
{
    public class OnPaymentSucceededOutboxMessageHandler: OutboxMessageHandlerBase<OnPaymentSucceededOutboxMessage>
    {
        private readonly QueueClient _queueClient;

        public OnPaymentSucceededOutboxMessageHandler(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public override async ValueTask OnOutboxMessageCreated(OnPaymentSucceededOutboxMessage outboxMessage, CancellationToken stoppingToken)
        {
            await _queueClient.SendMessageAsync(EventsConvert.Serialize(new OnPaymentSucceeded
            {
                OrderId = outboxMessage.OrderId
            }), stoppingToken);
        }
    }
}