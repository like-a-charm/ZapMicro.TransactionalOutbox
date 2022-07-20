using AutoMapper;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Profiles
{
    public class OrderServiceProfile: Profile
    {
        public OrderServiceProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderLine, OrderLineDto>().ReverseMap();
            CreateMap<Adjustment, AdjustmentDto>().ReverseMap();
        }
    }
}