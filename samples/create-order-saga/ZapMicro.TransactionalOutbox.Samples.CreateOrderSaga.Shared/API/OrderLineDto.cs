using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class OrderLineDto
    {
        public string ProductId { get; set; }
        public uint ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        public IEnumerable<AdjustmentDto> Adjustments { get; set; }
        public double Total { get; set; }
        public double FinalTotal {get; set; }
    }
}