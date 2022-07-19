using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.BackgroundServices
{
    internal class DequeueOutboxMessagesServiceWorker: IDequeueOutboxMessagesServiceWorker
    {
        
    private readonly IWithNextOutboxMessageCommand _withNextOutboxMessageCommand;
    private readonly IEnumerable<IOutboxMessageHandler> _outboxMessageHandlers;
    private readonly ILogger<DequeueOutboxMessagesServiceWorker> _logger;
    private readonly DequeueOutboxMessagesConfiguration _configuration;

    public DequeueOutboxMessagesServiceWorker(
        IWithNextOutboxMessageCommand withNextOutboxMessageCommand,
        IEnumerable<IOutboxMessageHandler> outboxMessageHandlers,
        ILogger<DequeueOutboxMessagesServiceWorker> logger,
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

    public virtual async ValueTask DequeueMessage(CancellationToken stoppingToken)
    {
        try
        {
            var message =
                await _withNextOutboxMessageCommand.WithNextIfNotNullAsync(HandleOutboxMessage, stoppingToken);
            if (message == null) await Delay(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"{nameof(DequeueOutboxMessagesServiceWorker)}.{nameof(DequeueMessage)}: Error while dequeuing outbox message");
        }
    }

    internal virtual Task Delay(CancellationToken stoppingToken) => Task.Delay(
        TimeSpan.FromSeconds(_configuration.EmptyQueueDelayInSeconds),
        stoppingToken);


    
    }
}