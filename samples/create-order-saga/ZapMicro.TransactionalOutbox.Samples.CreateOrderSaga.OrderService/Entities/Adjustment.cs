using System;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities
{
    public class Adjustment: EntityBase<Guid>
    {
        public string OfferId { get; set; }
        public double Total { get; set; }
    }
}