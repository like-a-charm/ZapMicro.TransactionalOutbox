using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Repositories
{
    public class RepositoryBase<TAggregate, TId> : IRepository<TAggregate, TId> where TAggregate : class, IAggregate<TId>
    {
        private readonly OrderServiceDbContext _orderServiceDbContext;

        public RepositoryBase(OrderServiceDbContext orderServiceDbContext)
        {
            _orderServiceDbContext = orderServiceDbContext; 
        }

        public Task<TAggregate> GetByIdAsync(TId id) =>
            _orderServiceDbContext.Set<TAggregate>().FirstOrDefaultAsync(x => x.Id.Equals(id));

        public async ValueTask CreateAsync(TAggregate aggregate)
        {
            aggregate.CreatedAt = aggregate.LastUpdatedAt = DateTime.Now;
            await _orderServiceDbContext.Set<TAggregate>().AddAsync(aggregate);
        }

        public void Update(TAggregate aggregate)
        {
            aggregate.LastUpdatedAt = DateTime.Now;
            _orderServiceDbContext.Set<TAggregate>().Update(aggregate);
        }
    }
}