using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.Entities;

namespace ZapMicro.TransactionalOutbox.DbContexts
{
    public interface ITransactionalOutboxDbContext
    {
        DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}