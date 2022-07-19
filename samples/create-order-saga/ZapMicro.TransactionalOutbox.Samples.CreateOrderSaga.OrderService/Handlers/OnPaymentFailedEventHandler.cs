using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Services;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Handlers
{
    public class OnPaymentFailedEventHandler : EventHandlerBase<OnPaymentFailed>
    {
        private IOrderService _orderService;

        public OnPaymentFailedEventHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public override async ValueTask HandleEvent(OnPaymentFailed @event)
        {
            await _orderService.UpdateOrderStatus(@event.OrderId, OrderStatus.Rejected);
        }
    }
}