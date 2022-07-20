using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.Events
{
    public class EventProcessingService: BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<QueueClient> _queueClientFactory;

        public EventProcessingService(IServiceProvider serviceProvider, Func<QueueClient> queueClientFactory)
        {
            _serviceProvider = serviceProvider;
            _queueClientFactory = queueClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var client = _queueClientFactory();
                    var eventHandlers = scope.ServiceProvider.GetRequiredService<IEventHandlers>();
                    try
                    {
                        var message = await client.ReceiveMessageAsync();
                        var @event = EventsConvert.Deserialize(message.Value.Body.ToString());
                        await eventHandlers.HandleEvent(@event);
                    }
                    catch (Exception)
                    {
                    }
                }
                
            }
        }
    }
}