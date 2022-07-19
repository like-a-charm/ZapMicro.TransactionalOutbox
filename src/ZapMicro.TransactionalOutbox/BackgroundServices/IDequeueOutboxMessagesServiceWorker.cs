using System.Threading;
using System.Threading.Tasks;

namespace ZapMicro.TransactionalOutbox.BackgroundServices
{
    internal interface IDequeueOutboxMessagesServiceWorker
    {
        public ValueTask DequeueMessage(CancellationToken token);
    }
}