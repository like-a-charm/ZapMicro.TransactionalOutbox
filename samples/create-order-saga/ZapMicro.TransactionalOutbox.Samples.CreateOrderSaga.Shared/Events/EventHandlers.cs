using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public class EventHandlers: IEventHandlers
    {
        private readonly IEnumerable<IEventHandler> _handlers;

        public EventHandlers(IEnumerable<IEventHandler> handlers)
        {
            _handlers = handlers;
        }

        public ValueTask HandleEvent(IEvent @event) =>
            _handlers.FirstOrDefault(x => x.ManagedEventType == @event.GetType())?.HandleEvent(@event) ??
            ValueTask.CompletedTask;
    }
}