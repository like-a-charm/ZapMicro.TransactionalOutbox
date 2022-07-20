using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Repositories;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Repositories
{
    public class PaymentRepository: RepositoryBase<Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(PaymentServiceDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Payment> IncludeDetails(DbSet<Payment> _dbSet)
        {
            return _dbSet;
        }
    }
}