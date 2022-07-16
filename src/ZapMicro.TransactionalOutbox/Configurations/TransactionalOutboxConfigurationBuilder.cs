using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Configurations
{
    internal class TransactionalOutboxConfigurationBuilder: ITransactionalOutboxConfigurationBuilder
    {
        private readonly IList<Func<IServiceProvider, IOutboxMessageHandler>> _outboxMessageHandlersFactories = new List<Func<IServiceProvider, IOutboxMessageHandler>>();
        private DequeueOutboxMessagesConfiguration _dequeueOutboxMessagesConfiguration = new DequeueOutboxMessagesConfiguration();
        
        public ITransactionalOutboxConfigurationBuilder ConfigureDequeueOutboxMessagesConfiguration(
            DequeueOutboxMessagesConfiguration configuration)
        {
            _dequeueOutboxMessagesConfiguration = configuration;
            return this;
        }

        public ITransactionalOutboxConfigurationBuilder ConfigureOutboxMessageHandler<THandler, TMessage>() where THandler : IOutboxMessageHandler<TMessage> where TMessage : IOutboxMessage
        {
            return ConfigureOutboxMessageHandler<THandler, TMessage>(sp =>
                ActivatorUtilities.CreateInstance<THandler>(sp));
        }

        public ITransactionalOutboxConfigurationBuilder ConfigureOutboxMessageHandler<THandler, TMessage>(Func<IServiceProvider, THandler> objectFactory) where THandler : IOutboxMessageHandler<TMessage> where TMessage : IOutboxMessage
        {
            _outboxMessageHandlersFactories.Add(sp => objectFactory(sp));
            return this;
        }

        ITransactionalOutboxConfiguration ITransactionalOutboxConfigurationBuilder.Build()
        {
            return new TransactionalOutboxConfiguration(this);
        }


        private class TransactionalOutboxConfiguration : ITransactionalOutboxConfiguration
        {
            private readonly TransactionalOutboxConfigurationBuilder _builder;

            public TransactionalOutboxConfiguration(TransactionalOutboxConfigurationBuilder builder)
            {
                _builder = builder;
            }


            public IEnumerable<Func<IServiceProvider, IOutboxMessageHandler>> OutboxMessageHandlersFactories =>
                _builder._outboxMessageHandlersFactories.AsEnumerable();

            public DequeueOutboxMessagesConfiguration DequeueOutboxMessagesConfiguration =>
                _builder._dequeueOutboxMessagesConfiguration;
        }
    }
}