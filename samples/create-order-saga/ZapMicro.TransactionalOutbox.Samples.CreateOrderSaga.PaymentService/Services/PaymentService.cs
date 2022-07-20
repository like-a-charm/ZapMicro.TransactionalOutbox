using System;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.DbContexts;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.OutboxMessages;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Repositories;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.PaymentService.Services
{
    public class PaymentService: IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentServiceDbContext _dbContext;
        private readonly IEnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;

        public PaymentService(IPaymentRepository paymentRepository, PaymentServiceDbContext dbContext, IEnqueueOutboxMessageCommand enqueueOutboxMessageCommand)
        {
            _paymentRepository = paymentRepository;
            _dbContext = dbContext;
            _enqueueOutboxMessageCommand = enqueueOutboxMessageCommand;
        }

        public async ValueTask RegisterPayment(Guid orderId, double paymentAmount, CancellationToken token)
        {
            try
            {
                if (paymentAmount < 0)
                {
                    await EnqueuePaymentFailedOutboxMessage(orderId, token);
                    return;
                }

                await _dbContext.Database.BeginTransactionAsync(token);
                await _paymentRepository.CreateAsync(new Payment
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    Amount = paymentAmount
                });
                await EnqueuePaymentSuccededOutboxMessage(orderId, token);
                await _dbContext.Database.CommitTransactionAsync(token);
            }
            catch (Exception)
            {
                await EnqueuePaymentFailedOutboxMessage(orderId, token);
            }
            finally
            {
                await _dbContext.SaveChangesAsync(token);
            }
        }

        private ValueTask EnqueuePaymentFailedOutboxMessage(Guid orderId, CancellationToken token)
        {
            return _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(new OnPaymentFailedOutboxMessage
            {
                OrderId = orderId
            }, token);
        }
        
        private ValueTask EnqueuePaymentSuccededOutboxMessage(Guid orderId, CancellationToken token)
        {
            return _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(new OnPaymentSucceededOutboxMessage
            {
                OrderId = orderId
            }, token);
        }
    }
}