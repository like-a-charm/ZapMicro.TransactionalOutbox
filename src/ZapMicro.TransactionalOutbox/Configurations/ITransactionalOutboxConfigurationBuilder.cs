using System;
using Microsoft.Extensions.DependencyInjection;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Configurations
{
    public interface ITransactionalOutboxConfigurationBuilder
    {
        ITransactionalOutboxConfigurationBuilder ConfigureDequeueOutboxMessagesConfiguration(
            DequeueOutboxMessagesConfiguration configuration);

        ITransactionalOutboxConfigurationBuilder ConfigureOutboxMessageHandler<THandler, TMessage>() 
            where TMessage : IOutboxMessage
            where THandler : IOutboxMessageHandler<TMessage>;
        
        ITransactionalOutboxConfigurationBuilder ConfigureOutboxMessageHandler<THandler, TMessage>(Func<IServiceProvider, THandler> objectFactory) 
            where TMessage : IOutboxMessage
            where THandler : IOutboxMessageHandler<TMessage>;

        internal ITransactionalOutboxConfiguration Build();
    }
}