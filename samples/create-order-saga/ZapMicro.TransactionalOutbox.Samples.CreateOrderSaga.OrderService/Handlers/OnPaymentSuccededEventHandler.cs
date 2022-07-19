using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Services;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Handlers
{
    public class OnPaymentSuccededEventHandler : EventHandlerBase<OnPaymentSucceded>
    {
        private IOrderService _orderService;

        public OnPaymentSuccededEventHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public override async ValueTask HandleEvent(OnPaymentSucceded @event)
        {
            await _orderService.UpdateOrderStatus(@event.OrderId, OrderStatus.Confirmed);
        }
    }
}