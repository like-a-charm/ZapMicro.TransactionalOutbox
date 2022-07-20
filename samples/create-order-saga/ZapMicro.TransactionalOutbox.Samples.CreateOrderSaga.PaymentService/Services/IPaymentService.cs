using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Services
{
    public interface IPaymentService
    {
        ValueTask RegisterPayment(Guid orderId, double paymentAmount, CancellationToken token);
    }
}