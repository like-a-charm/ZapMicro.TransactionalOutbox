using System;
using System.Collections.Generic;
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

        public EventProcessingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var client = scope.ServiceProvider.GetRequiredService<QueueClient>();
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