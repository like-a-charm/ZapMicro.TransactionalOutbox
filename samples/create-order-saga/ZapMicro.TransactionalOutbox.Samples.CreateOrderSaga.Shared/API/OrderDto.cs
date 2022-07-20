using System;
using System.Collections.Generic;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public IEnumerable<OrderLineDto> Lines { get; set; }
        public IList<AdjustmentDto> Adjustments { get; set; }
        public double Total { get; set; }
        public double FinalTotal { get; set; }
        public OrderStatusDto Status { get; set; }
    }
}