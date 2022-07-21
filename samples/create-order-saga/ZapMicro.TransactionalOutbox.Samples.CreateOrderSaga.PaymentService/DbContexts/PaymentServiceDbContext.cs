using System;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Entities;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.DbContexts
{
    public class PaymentServiceDbContext: DbContext, ITransactionalOutboxDbContext
    {
        public PaymentServiceDbContext(DbContextOptions options):base(options)
        {
        }
        
        public PaymentServiceDbContext(): this(
            new DbContextOptionsBuilder()
                .UseSqlServer(Environment.GetEnvironmentVariable("PAYMENTS_DATABASE_CONNECTION_STRING")!)
                .Options)
        {
        }

        public DbSet<OutboxMessage> OutboxMessages { get; }
        public DbSet<Payment> Payments { get; }
    }
}