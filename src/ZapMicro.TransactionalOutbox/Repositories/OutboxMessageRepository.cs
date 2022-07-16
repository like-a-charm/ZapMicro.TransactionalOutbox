using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Repositories
{
    internal class OutboxMessageRepository<T>: IOutboxMessageRepository where T: DbContext, ITransactionalOutboxDbContext
    {
        private readonly T _transactionalOutboxDbContext;

        public OutboxMessageRepository(T transactionalOutboxDbContext)
        {
            _transactionalOutboxDbContext = transactionalOutboxDbContext;
        }

        public async ValueTask CreateAsync(OutboxMessage message, CancellationToken token)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            await _transactionalOutboxDbContext.OutboxMessages.AddAsync(message, token);
        }

        public Task<OutboxMessage?> GetFirstAsync(CancellationToken token) => 
            _transactionalOutboxDbContext.OutboxMessages.AsQueryable().OrderBy(x => x.CreatedAt).FirstOrDefaultAsync(token);

        public void Delete(OutboxMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _transactionalOutboxDbContext.OutboxMessages.Remove(message);
        }

        public Task SaveChangesAsync(CancellationToken token) => _transactionalOutboxDbContext.SaveChangesAsync(token);
    }
}