using System;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Handlers
{
    public abstract class OutboxMessageHandlerBase<T>: IOutboxMessageHandler<T> where T: IOutboxMessage
    {
        public abstract ValueTask OnOutboxMessageCreated(T outboxMessage, CancellationToken stoppingToken);

        ValueTask IOutboxMessageHandler.OnOutboxMessageCreated(IOutboxMessage outboxMessage, CancellationToken stoppingToken)
        {
            if (outboxMessage is not T castedOutboxMessage) throw new ArgumentException($"{nameof(outboxMessage)} must be instance of {typeof(T)}");
            return OnOutboxMessageCreated(castedOutboxMessage, stoppingToken);
        }

        Type IOutboxMessageHandler.ManagedOutboxMessageType => typeof(T);
    }
}