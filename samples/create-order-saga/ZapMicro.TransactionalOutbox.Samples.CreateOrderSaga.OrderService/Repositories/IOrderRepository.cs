using System;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Repositories;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Repositories
{
    public interface IOrderRepository: IRepository<Order, Guid>
    {
        
    }
}