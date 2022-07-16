using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ZapMicro.TransactionalOutbox.BackgroundServices;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Converters;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTransactionalOutbox<T>(
            this IServiceCollection services,
            Func<ITransactionalOutboxConfigurationBuilder, ITransactionalOutboxConfigurationBuilder>
                configurationBuilderDirector) where T: DbContext, ITransactionalOutboxDbContext
        {
            if (configurationBuilderDirector == null)
                throw new ArgumentNullException(nameof(configurationBuilderDirector));
            
            var configuration = configurationBuilderDirector.Invoke(new TransactionalOutboxConfigurationBuilder())
                .Build();

            services
                .AddScoped<IOutboxMessageConverter, JsonNetOutboxMessageConverter>()
                .AddSingleton(configuration.DequeueOutboxMessagesConfiguration)
                .AddScoped<IOutboxMessageRepository, OutboxMessageRepository<T>>()
                .AddScoped<IWithNextOutboxMessageCommand, WithNextOutboxMessageCommand>()
                .AddScoped<IEnqueueOutboxMessageCommand, EnqueueOutboxMessageCommand>()
                .AddScoped<IHostedService, DequeueOutboxMessagesService>();
            
            configuration.OutboxMessageHandlersFactories
                .ToList()
                .ForEach(x => services.AddScoped(x));
            
            return services;
        }
    }
}