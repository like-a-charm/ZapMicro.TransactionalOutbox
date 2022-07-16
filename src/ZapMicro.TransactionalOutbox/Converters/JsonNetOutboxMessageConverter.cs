using Newtonsoft.Json;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Converters
{
    internal class JsonNetOutboxMessageConverter: IOutboxMessageConverter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonNetOutboxMessageConverter()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public string Serialize(IOutboxMessage message) => JsonConvert.SerializeObject(message, _jsonSerializerSettings);

        public IOutboxMessage Deserialize(string serializedMessage) =>
            JsonConvert.DeserializeObject<IOutboxMessage>(serializedMessage, _jsonSerializerSettings)!;
    }
}