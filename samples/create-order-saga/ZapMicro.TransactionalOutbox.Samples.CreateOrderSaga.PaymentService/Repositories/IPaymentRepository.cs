using System;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Repositories;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Repositories
{
    public interface IPaymentRepository: IRepository<Payment, Guid>
    {
        
    }
}