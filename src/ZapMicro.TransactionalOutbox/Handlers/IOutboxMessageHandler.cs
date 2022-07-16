using System;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Handlers
{
    public interface IOutboxMessageHandler
    {
        internal ValueTask OnOutboxMessageCreated(IOutboxMessage outboxMessage, CancellationToken stoppingToken);
        internal Type ManagedOutboxMessageType { get; }
    }
    public interface IOutboxMessageHandler<in T> : IOutboxMessageHandler where T: IOutboxMessage
    {
        ValueTask OnOutboxMessageCreated(T outboxMessage, CancellationToken stoppingToken);
    }
}