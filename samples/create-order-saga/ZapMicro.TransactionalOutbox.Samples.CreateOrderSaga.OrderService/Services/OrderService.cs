using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Repositories;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Services
{
    public class OrderService: IOrderService
    {
        private readonly IRepository<Order, Guid> _repository;
        private readonly IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;
        private readonly OrderServiceDbContext _orderServiceDbContext;

        public OrderService(IRepository<Order, Guid> repository, IEnqueueOutboxMessageCommand enqueueOutboxMessageCommand, OrderServiceDbContext orderServiceDbContext)
        {
            _repository = repository;
            _enqueueOutboxMessageCommand = enqueueOutboxMessageCommand;
            _orderServiceDbContext = orderServiceDbContext;
        }


        public async Task<Order> CreateOrder(IEnumerable<OrderLine> lines, IEnumerable<Adjustment> adjustments)
        {
            await _orderServiceDbContext.Database.BeginTransactionAsync();
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Lines = lines.ToList(),
                Adjustments = adjustments.ToList()
            };

            CreateIds(order);
            await _repository.CreateAsync(order);
            await _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(new OnOrderCreatedOutboxMessage
            {
                OrderId = order.Id,
                OrderGrandTotal = order.FinalTotal
            }, CancellationToken.None);
            
            await _orderServiceDbContext.Database.CommitTransactionAsync();
            return order;
        }

        public Task<Order> GetOrderById(Guid orderId)
        {
            return _repository.GetByIdAsync(orderId);
        }

        public async Task UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
            var order = await GetOrderById(orderId);
            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException(
                    $"Cannot change status of a non pending order. Order id: {orderId}, current status: {order.Status}");
            }
            order.Status = newStatus;
            _repository.Update(order);
        }

        private void CreateIds(Order order)
        {
            foreach (var line in order.Lines)
            {
                line.Id = Guid.NewGuid();
                foreach (var adjustment in line.Adjustments)
                {
                    adjustment.Id = Guid.NewGuid();
                }
            }

            foreach (var adjustment in order.Adjustments)
            {
                adjustment.Id = Guid.NewGuid();
            }
        }
    }
}