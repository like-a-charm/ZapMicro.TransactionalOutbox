namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class AdjustmentDto
    {
        public string OfferId { get; set; }
        public decimal Total { get; set; }
    }
}