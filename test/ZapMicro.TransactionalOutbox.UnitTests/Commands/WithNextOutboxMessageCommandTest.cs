using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Converters;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Messages;
using ZapMicro.TransactionalOutbox.Repositories;

namespace ZapMicro.TransactionalOutbox.UnitTests.Commands
{
    public class WithNextOutboxMessageCommandTest
    {
        private WithNextOutboxMessageCommand _withNextOutboxMessageCommand;
        private IOutboxMessageConverter _outboxMessageConverter;
        private IOutboxMessageRepository _outboxMessageRepository;

        private int _delegateReceivedCalls;

        [SetUp]
        public void SetUp()
        {
            _outboxMessageConverter = Substitute.For<IOutboxMessageConverter>();
            _outboxMessageRepository = Substitute.For<IOutboxMessageRepository>();
            _withNextOutboxMessageCommand = new WithNextOutboxMessageCommand(_outboxMessageConverter, _outboxMessageRepository);
            _delegateReceivedCalls = 0;
        }

        public ValueTask AnAction(IOutboxMessage message, CancellationToken token)
        {
            _delegateReceivedCalls++;
            return ValueTask.CompletedTask;
        }

        [Test]
        public async Task WithNextOutboxMessageIfNotNull_InvokesDelegate_IfNextOutboxMessageExists()
        {
            var message = new TestOutboxMessage
            {
                Param1 = string.Empty
            };
            var rawMessage = new OutboxMessage
            {
                CreatedAt = new DateTime(2020,1,1),
                Id= Guid.Empty,
                Payload = string.Empty
            };

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var getFirstAsyncResult = Task.FromResult(rawMessage);
            
            _outboxMessageConverter.Deserialize(Arg.Any<string>()).Returns(message);
            _outboxMessageRepository.GetFirstAsync(cancellationToken).Returns(rawMessage);

            (await _withNextOutboxMessageCommand.WithNextIfNotNullAsync(AnAction, cancellationToken))
                .Should().NotBeNull();
            
            _delegateReceivedCalls.Should().Be(1);
            
            _outboxMessageConverter.Received(1).Deserialize(Arg.Any<string>());
            _outboxMessageRepository.Received(1).GetFirstAsync(cancellationToken);
        }
        
        [Test]
        public async Task WithNextOutboxMessageIfNotNull_DoesntInvokeDelegate_IfNextOutboxMessageIsNull()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var getFirstAsyncResult = Task.FromResult(default(OutboxMessage));
            
            _outboxMessageRepository.GetFirstAsync(cancellationToken).Returns(getFirstAsyncResult);

            (await _withNextOutboxMessageCommand.WithNextIfNotNullAsync(AnAction, cancellationToken))
                .Should().BeNull();
            
            _delegateReceivedCalls.Should().Be(0);
            
            _outboxMessageRepository.Received(1).GetFirstAsync(cancellationToken);
        }
    }
}