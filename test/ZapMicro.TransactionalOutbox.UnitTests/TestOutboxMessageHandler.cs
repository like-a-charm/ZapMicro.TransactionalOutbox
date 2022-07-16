using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.UnitTests
{
    public class TestOutboxMessageHandler: OutboxMessageHandlerBase<TestOutboxMessage>
    {
        public override ValueTask OnOutboxMessageCreated(TestOutboxMessage outboxMessage, CancellationToken stoppingToken)
        {
            return ValueTask.CompletedTask;
        }
    }
}