using System;
using System.Collections.Generic;
using System.Linq;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities
{
    public class Order: AggregateBase<Guid>
    {
        public OrderStatus Status { get; set; }
        public IList<OrderLine> Lines { get; set; }
        public IList<Adjustment> Adjustments { get; set; }
        public decimal Total => Lines?.Sum(x => x.FinalTotal) ?? 0;
        public decimal FinalTotal => Total + Adjustments?.Sum(x => x.Total) ?? 0;
    }
}