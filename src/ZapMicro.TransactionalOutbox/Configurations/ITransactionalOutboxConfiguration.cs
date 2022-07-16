using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;

namespace ZapMicro.TransactionalOutbox.Configurations
{
    internal interface ITransactionalOutboxConfiguration
    {
        IEnumerable<Func<IServiceProvider, IOutboxMessageHandler>> OutboxMessageHandlersFactories { get; }
        DequeueOutboxMessagesConfiguration DequeueOutboxMessagesConfiguration { get; }
    }
}