using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(IEnumerable<OrderLine> lines, IEnumerable<Adjustment> adjustments);
        Task<Order> GetOrderById(Guid orderId);
        Task UpdateOrderStatus(Guid orderId, OrderStatus newStatus);
    }
}