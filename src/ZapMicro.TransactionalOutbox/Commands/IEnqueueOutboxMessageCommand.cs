using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Commands
{
    public interface IEnqueueOutboxMessageCommand
    {
        ValueTask EnqueueOutboxMessageAsync(IOutboxMessage message, CancellationToken token);
    }
}