using System;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Commands
{
    internal interface IWithNextOutboxMessageCommand
    {
        ValueTask<IOutboxMessage?> WithNextIfNotNullAsync(Func<IOutboxMessage, CancellationToken, ValueTask> action, CancellationToken token);
    }
}