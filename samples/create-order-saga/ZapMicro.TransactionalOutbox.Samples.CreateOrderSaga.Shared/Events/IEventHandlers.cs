using System.Threading.Tasks;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public interface IEventHandlers
    {
        ValueTask HandleEvent(IEvent @event);
    }
}