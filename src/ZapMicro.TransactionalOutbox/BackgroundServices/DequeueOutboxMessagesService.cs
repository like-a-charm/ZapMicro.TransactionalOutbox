using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.BackgroundServices
{
    internal class DequeueOutboxMessagesService: BackgroundService
    {
        private readonly IWithNextOutboxMessageCommand _withNextOutboxMessageCommand;
        private readonly IEnumerable<IOutboxMessageHandler> _outboxMessageHandlers;
        private readonly ILogger<DequeueOutboxMessagesService> _logger;
        private readonly DequeueOutboxMessagesConfiguration _configuration;

        public DequeueOutboxMessagesService(
            IWithNextOutboxMessageCommand withNextOutboxMessageCommand,
            IEnumerable<IOutboxMessageHandler> outboxMessageHandlers,
            ILogger<DequeueOutboxMessagesService> logger,
            DequeueOutboxMessagesConfiguration configuration)
        {
            _withNextOutboxMessageCommand = withNextOutboxMessageCommand;
            _outboxMessageHandlers = outboxMessageHandlers;
            _logger = logger;
            _configuration = configuration;
        }

        internal virtual ValueTask HandleOutboxMessage(IOutboxMessage message, CancellationToken token) =>

            _outboxMessageHandlers
                .FirstOrDefault(x => x.ManagedOutboxMessageType == message.GetType())?
                .OnOutboxMessageCreated(message, token) ?? ValueTask.CompletedTask;

        internal virtual async ValueTask DequeueMessage(CancellationToken stoppingToken)
        {
            try
            {
                var message = await _withNextOutboxMessageCommand.WithNextIfNotNullAsync(HandleOutboxMessage, stoppingToken);
                if (message == null) await Delay(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(DequeueOutboxMessagesService)}.{nameof(DequeueMessage)}: Error while dequeuing outbox message");
            }
        }

        internal virtual Task Delay(CancellationToken stoppingToken) => Task.Delay(
            TimeSpan.FromSeconds(_configuration.EmptyQueueDelayInSeconds),
            stoppingToken);

        internal virtual bool IsCancellationRequested(CancellationToken stoppingToken) =>
            stoppingToken.IsCancellationRequested;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!IsCancellationRequested(stoppingToken))
            {
                await DequeueMessage(stoppingToken);
            }
        }
    }
}