using System;
using System.Threading.Tasks;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public abstract class EventHandlerBase<T> : IEventHandler<T> where T : IEvent
    {
        public abstract ValueTask HandleEvent(T @event);

        Type IEventHandler.ManagedEventType => typeof(T);

        ValueTask IEventHandler.HandleEvent(IEvent @event) => HandleEvent((T) @event);
        
    }
}