using System.Collections.Generic;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class CreateOrderRequest
    {
        public IEnumerable<AdjustmentDto> Adjustments { get; set; }
        public IEnumerable<OrderLineDto> Lines { get; set; }
        
    }
}