using System;
using System.Collections.Generic;
using System.Linq;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities
{
    public class OrderLine: EntityBase<Guid>
    {
        public string ProductId { get; set; }
        public uint ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public IList<Adjustment> Adjustments { get; set; }
        public decimal Total => ProductQuantity * ProductPrice;
        public decimal FinalTotal => Total + Adjustments?.Sum(x => x.Total) ?? 0;

    }
}