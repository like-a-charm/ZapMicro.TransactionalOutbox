using System;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities
{
    public class Adjustment: EntityBase<Guid>
    {
        public string OfferId { get; set; }
        public decimal Total { get; set; }
    }
}