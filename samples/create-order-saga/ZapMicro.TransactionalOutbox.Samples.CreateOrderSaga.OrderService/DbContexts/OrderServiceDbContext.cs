using System;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts
{
    public class OrderServiceDbContext: DbContext, ITransactionalOutboxDbContext
    {
        public OrderServiceDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public OrderServiceDbContext() : this(
            new DbContextOptionsBuilder()
                .UseSqlServer(Environment.GetEnvironmentVariable("ORDERS_DATABASE_CONNECTION_STRING")!)
                .Options)
        {
        }

        public DbSet<OutboxMessage> OutboxMessages { get; }
        public DbSet<Order> Orders { get; }
        public DbSet<OrderLine> OrderLines { get; }
        public DbSet<Adjustment> Adjustments { get; }
    }
}