using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Entities;

namespace ZapMicro.TransactionalOutbox.UnitTests
{
    public class TestTransactionalOutboxDbContext: DbContext, ITransactionalOutboxDbContext
    {
        private readonly static DbContextOptions _options = new DbContextOptionsBuilder<TestTransactionalOutboxDbContext>()
            .UseInMemoryDatabase(nameof(TestTransactionalOutboxDbContext))
            .Options;
        
        public TestTransactionalOutboxDbContext(): base(_options)
        {
            
        }
        public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}