using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Repositories
{
    public interface IRepository<TAggregate, in TId> where TAggregate : class, IAggregate<TId>
    {
        Task<TAggregate> GetByIdAsync(TId id);
        ValueTask CreateAsync(TAggregate aggregate);
        void Update(TAggregate aggregate);
    }
}