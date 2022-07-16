using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Converters
{
    internal interface IOutboxMessageConverter
    {
        string Serialize(IOutboxMessage message);
        IOutboxMessage Deserialize(string serializedMessage);
    }
}