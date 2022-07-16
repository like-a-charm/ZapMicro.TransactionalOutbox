using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.UnitTests
{
    public class TestOutboxMessage: IOutboxMessage
    {
        public string? Param1 { get; set; }
    }
}