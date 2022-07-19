using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ZapMicro.TransactionalOutbox.BackgroundServices
{
    internal class DequeueOutboxMessagesService : BackgroundService
    {
        private IServiceProvider _serviceProvider;

        public DequeueOutboxMessagesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!IsCancellationRequested(stoppingToken))
            {
                using var scope = _serviceProvider.CreateScope();
                var dequeueOutboxMessagesServiceWorker =
                    scope.ServiceProvider.GetRequiredService<IDequeueOutboxMessagesServiceWorker>();
                await dequeueOutboxMessagesServiceWorker.DequeueMessage(stoppingToken);
            }
        }

        internal virtual bool IsCancellationRequested(CancellationToken stoppingToken) =>
            stoppingToken.IsCancellationRequested;
    }
}