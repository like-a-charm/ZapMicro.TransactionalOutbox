using System;
using System.Threading.Tasks;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public interface IEventHandler
    {
        internal Type ManagedEventType { get; }
        internal ValueTask HandleEvent(IEvent @event);
    }

    public interface IEventHandler<in T> : IEventHandler where T : IEvent
    {
        ValueTask HandleEvent(T @event);
    }
}