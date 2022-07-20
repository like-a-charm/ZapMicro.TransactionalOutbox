using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Repositories
{
    public abstract class RepositoryBase<TAggregate, TId> : IRepository<TAggregate, TId> where TAggregate : class, IAggregate<TId>
    {
        private readonly DbContext _dbContext;

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public Task<TAggregate> GetByIdAsync(TId id) =>
            IncludeDetails(_dbContext.Set<TAggregate>()).FirstOrDefaultAsync(x => x.Id.Equals(id));

        public async ValueTask CreateAsync(TAggregate aggregate)
        {
            aggregate.CreatedAt = aggregate.LastUpdatedAt = DateTime.Now;
            await _dbContext.Set<TAggregate>().AddAsync(aggregate);
        }

        public void Update(TAggregate aggregate)
        {
            aggregate.LastUpdatedAt = DateTime.Now;
            _dbContext.Set<TAggregate>().Update(aggregate);
        }

        protected abstract IQueryable<TAggregate> IncludeDetails(DbSet<TAggregate> _dbSet);

    }
}