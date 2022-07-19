using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Repositories
{
    public class Repository<TAggregate, TId> : RepositoryBase<TAggregate, TId> where TAggregate : class, IAggregate<TId>
    {
        public Repository(OrderServiceDbContext orderServiceDbContext) : base(orderServiceDbContext)
        {
        }
    }
}