using System;
using System.Collections.Generic;
using System.Linq;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Entities;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities
{
    public class OrderLine: EntityBase<Guid>
    {
        public string ProductId { get; set; }
        public uint ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        public IList<Adjustment> Adjustments { get; set; }
        public double Total => ProductQuantity * ProductPrice;
        public double FinalTotal => Total + Adjustments?.Sum(x => x.Total) ?? 0;

    }
}