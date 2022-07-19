using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Handlers
{
    public class OnOrderCreatedOutboxMessageHandler: OutboxMessageHandlerBase<OnOrderCreatedOutboxMessage>
    {
        private ILogger<OnOrderCreatedOutboxMessageHandler> _logger;
        private QueueClient _queueClient;

        public OnOrderCreatedOutboxMessageHandler(ILogger<OnOrderCreatedOutboxMessageHandler> logger, QueueClient queueClient)
        {
            _logger = logger;
            _queueClient = queueClient;
        }

        public override async ValueTask OnOutboxMessageCreated(OnOrderCreatedOutboxMessage outboxMessage, CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Handling message for order {outboxMessage.OrderId}");
            var @event = new OnOrderCreated
            {
                Id = Guid.NewGuid(),
                OrderId = outboxMessage.OrderId,
                OrderGrandTotal = outboxMessage.OrderGrandTotal
            };
            await _queueClient.SendMessageAsync(EventsConvert.Serialize(@event), stoppingToken);
        }
    }
}