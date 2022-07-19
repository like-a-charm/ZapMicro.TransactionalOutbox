using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class OrderLineDto
    {
        public string ProductId { get; set; }
        public uint ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public IEnumerable<AdjustmentDto> Adjustments { get; set; }
        public decimal Total { get; set; }
        public decimal FinalTotal {get; set; }
    }
}