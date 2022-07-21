using System;
using System.Threading.Tasks;
using RestEase;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.TestRunner
{
    [BasePath("order")]
    public interface IOrderServiceClient
    {
        [Post]
        Task<OrderDto> CreateOrderAsync([Body] CreateOrderRequest request);
        
        [Get("{orderId}")]
        Task<OrderDto> GetOrder([Path] Guid orderId);
    }
}