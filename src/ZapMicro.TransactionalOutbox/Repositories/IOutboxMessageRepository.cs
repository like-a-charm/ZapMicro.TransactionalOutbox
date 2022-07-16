using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Entities;

namespace ZapMicro.TransactionalOutbox.Repositories
{
    internal interface IOutboxMessageRepository
    {
        ValueTask CreateAsync(OutboxMessage message, CancellationToken token);
        Task<OutboxMessage?> GetFirstAsync(CancellationToken token);
        void Delete(OutboxMessage message);
        Task SaveChangesAsync(CancellationToken token);
    }
}