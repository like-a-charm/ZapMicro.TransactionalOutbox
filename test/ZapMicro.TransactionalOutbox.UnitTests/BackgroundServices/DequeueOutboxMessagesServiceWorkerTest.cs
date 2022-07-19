using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.BackgroundServices;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Configurations;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;
using static ZapMicro.TransactionalOutbox.UnitTests.Utils;

namespace ZapMicro.TransactionalOutbox.UnitTests.BackgroundServices
{

    internal class TestLogger : ILogger<DequeueOutboxMessagesServiceWorker>
    {
        private ILogger<DequeueOutboxMessagesServiceWorker> _loggerImplementation;

        public TestLogger(ILogger<DequeueOutboxMessagesServiceWorker> loggerImplementation)
        {
            _loggerImplementation = loggerImplementation;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            HasLogMethodBeenCalled = true;
            _loggerImplementation.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _loggerImplementation.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _loggerImplementation.BeginScope(state);
        }

        public bool HasLogMethodBeenCalled { get; private set; }
    }
    
    public class DequeueOutboxMessagesServiceWorkerTest
    {
        
        private DequeueOutboxMessagesServiceWorker? _dequeueOutboxMessagesServiceWorker;
        private IWithNextOutboxMessageCommand? _withNextOutboxMessageCommand;
        private ILogger<DequeueOutboxMessagesServiceWorker>? _logger;
        private List<IOutboxMessageHandler>? _handlers;

        [SetUp]
        public void SetUp()
        {
            _withNextOutboxMessageCommand = Substitute.For<IWithNextOutboxMessageCommand>();
            _handlers = new List<IOutboxMessageHandler>();
            _logger = new TestLogger(Substitute.For<ILogger<DequeueOutboxMessagesServiceWorker>>());
            var configuration = new DequeueOutboxMessagesConfiguration
            {
                EmptyQueueDelayInSeconds = 1
            };
            _dequeueOutboxMessagesServiceWorker = Substitute.ForPartsOf<DequeueOutboxMessagesServiceWorker>(_withNextOutboxMessageCommand, _handlers, _logger, configuration);

        }

        [Test]
        public void HandleOutboxMessage_ShouldHandleOutboxMessage_IfMessageHandlerExists()
        {
            var messageHandler = Substitute.For<IOutboxMessageHandler>();
            var managedOutboxMessageType = typeof(TestOutboxMessage);
            
            _handlers.Add(messageHandler);

            messageHandler.ManagedOutboxMessageType.Returns(managedOutboxMessageType);

            Act(async () =>
                    await _dequeueOutboxMessagesServiceWorker.HandleOutboxMessage(new TestOutboxMessage(),
                        new CancellationToken()))
                .Should().NotThrow();

            messageHandler.Received(1).OnOutboxMessageCreated(Arg.Any<IOutboxMessage>(), Arg.Any<CancellationToken>());
        }
        
        [Test]
        public void HandleOutboxMessage_ShouldNotThrow_IfMessageHandlerDoesntExist()
        {
            Act(async () =>
                    await _dequeueOutboxMessagesServiceWorker.HandleOutboxMessage(new TestOutboxMessage(),
                        new CancellationToken()))
                .Should().NotThrow();
        }
        
        [Test]
        public void Delay_ShouldNotThrow()
        {
            Act(async () =>
                    await _dequeueOutboxMessagesServiceWorker.Delay(new CancellationToken()))
                .Should().NotThrow();
        }

        [Test]
        public void DequeueMessage_ShouldInvokeHandleMessage_IfNextOutboxMessageExists()
        {
            _withNextOutboxMessageCommand
                .WithNextIfNotNullAsync(Arg.Any<Func<IOutboxMessage, CancellationToken, ValueTask>>(),
                    Arg.Any<CancellationToken>())
                .Returns(info =>
                {
                    info.ArgAt<Func<IOutboxMessage, CancellationToken, ValueTask>>(0)
                        .Invoke(new TestOutboxMessage(), info.ArgAt<CancellationToken>(1));
                    return new TestOutboxMessage();
                });

            _dequeueOutboxMessagesServiceWorker.HandleOutboxMessage(Arg.Any<IOutboxMessage>(), Arg.Any<CancellationToken>())
                .Returns(ValueTask.CompletedTask);

            Act(async () => await _dequeueOutboxMessagesServiceWorker.DequeueMessage(new CancellationToken()))
                .Should().NotThrow();

            _dequeueOutboxMessagesServiceWorker.Received(1)
                .HandleOutboxMessage(Arg.Any<IOutboxMessage>(), Arg.Any<CancellationToken>());
        }
        
        [Test]
        public void DequeueMessage_ShouldInvokeDelay_IfNextOutboxMessageDoesntExist()
        {
            _withNextOutboxMessageCommand
                .WithNextIfNotNullAsync(Arg.Any<Func<IOutboxMessage, CancellationToken, ValueTask>>(),
                    Arg.Any<CancellationToken>())
                .Returns(default(IOutboxMessage));

            _dequeueOutboxMessagesServiceWorker.Delay(Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            Act(async () => await _dequeueOutboxMessagesServiceWorker.DequeueMessage(new CancellationToken()))
                .Should().NotThrow();

            _dequeueOutboxMessagesServiceWorker.Received(1)
                .Delay(Arg.Any<CancellationToken>());
        }
        
        [Test]
        public void DequeueMessage_LogsError_IfExceptionIsThrown()
        {
            _withNextOutboxMessageCommand
                .WithNextIfNotNullAsync(Arg.Any<Func<IOutboxMessage, CancellationToken, ValueTask>>(),
                    Arg.Any<CancellationToken>())
                .Throws(new Exception());

            Act(async () => await _dequeueOutboxMessagesServiceWorker.DequeueMessage(new CancellationToken()))
                .Should().NotThrow();

            ((TestLogger)_logger).HasLogMethodBeenCalled.Should().BeTrue();
        }
        
    }
}