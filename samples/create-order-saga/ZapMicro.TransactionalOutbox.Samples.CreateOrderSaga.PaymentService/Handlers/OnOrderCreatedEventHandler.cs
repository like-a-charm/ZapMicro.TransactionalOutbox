using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Services;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Handlers
{
    public class OnOrderCreatedEventHandler : EventHandlerBase<OnOrderCreated>
    {
        private readonly IPaymentService _paymentService;

        public OnOrderCreatedEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public override ValueTask HandleEvent(OnOrderCreated @event) =>
            _paymentService.RegisterPayment(@event.OrderId, @event.OrderGrandTotal, CancellationToken.None);
    }
}