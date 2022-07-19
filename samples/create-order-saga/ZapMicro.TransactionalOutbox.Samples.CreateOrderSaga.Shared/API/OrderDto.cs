using System;
using System.Collections.Generic;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public IEnumerable<OrderLineDto> Lines { get; set; }
        public IList<AdjustmentDto> Adjustments { get; set; }
        public decimal Total { get; set; }
        public decimal FinalTotal { get; set; }
    }
}