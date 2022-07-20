using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.DbContexts
{
    public class PaymentServiceDbContext: DbContext, ITransactionalOutboxDbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}