using System;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Handlers;
using ZapMicro.TransactionalOutbox.Messages;
using static ZapMicro.TransactionalOutbox.UnitTests.Utils;

namespace ZapMicro.TransactionalOutbox.UnitTests.Handlers
{
    public class OutboxMessageHandlerBaseTest
    {
        private OutboxMessageHandlerBase<TestOutboxMessage>? _outboxMessageHandlerBase;

        [SetUp]
        public void SetUp()
        {
            _outboxMessageHandlerBase = Substitute.ForPartsOf<OutboxMessageHandlerBase<TestOutboxMessage>>();
        }

        [Test]
        public void ManagedOutboxMessageType_ShouldReturnTypeOfTestOutboxMessage()
        {
            ((IOutboxMessageHandler)_outboxMessageHandlerBase!).ManagedOutboxMessageType.Should().Be(typeof(TestOutboxMessage));
        }
        
        [Test]
        public void OnOutboxMessageCreated_ShouldInvokeGenericMethod_IfOutboxMessageIsOfManagedType()
        {
            Act(async () => await ((IOutboxMessageHandler)_outboxMessageHandlerBase!).OnOutboxMessageCreated(new TestOutboxMessage(),
                new CancellationToken()))
                .Should().NotThrow();

            _outboxMessageHandlerBase!.Received(1)
                .OnOutboxMessageCreated(Arg.Any<TestOutboxMessage>(), Arg.Any<CancellationToken>());

        }
        
        [Test]
        public void OnOutboxMessageCreated_ShouldThrowArgumentException_IfOutboxMessageIsNotOfManagedType()
        {
            Act(async () => await ((IOutboxMessageHandler)_outboxMessageHandlerBase!).OnOutboxMessageCreated(Substitute.For<IOutboxMessage>(),
                    new CancellationToken()))
                .Should().Throw<ArgumentException>();
        }
        
    }
}