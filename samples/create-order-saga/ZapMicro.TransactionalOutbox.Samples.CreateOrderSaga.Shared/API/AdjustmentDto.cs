namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API
{
    public class AdjustmentDto
    {
        public string OfferId { get; set; }
        public double Total { get; set; }
    }
}