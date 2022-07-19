using Newtonsoft.Json;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public static class EventsConvert
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.All
        };

        
        public static IEvent Deserialize(string jsonEvent)
        {
            return JsonConvert.DeserializeObject<IEvent>(jsonEvent)!;
        }
        
        public static string Serialize(IEvent @event)
        {
            return JsonConvert.SerializeObject(@event);
        }
    }
}