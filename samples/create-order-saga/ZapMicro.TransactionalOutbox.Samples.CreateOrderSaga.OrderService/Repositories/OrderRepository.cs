using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Repositories;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Repositories
{
    public class OrderRepository: RepositoryBase<Order, Guid>, IOrderRepository
    {
        public OrderRepository(OrderServiceDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Order> IncludeDetails(DbSet<Order> _dbSet)
        {
            return _dbSet
                .Include(x => x.Adjustments)
                .Include(x => x.Lines)
                .Include(x => x.Lines.Select(y => y.Adjustments));
        }
    }
}