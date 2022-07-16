using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Commands;
using ZapMicro.TransactionalOutbox.Converters;
using ZapMicro.TransactionalOutbox.Entities;
using static ZapMicro.TransactionalOutbox.UnitTests.Utils;
using ZapMicro.TransactionalOutbox.Repositories;

namespace ZapMicro.TransactionalOutbox.UnitTests.Commands
{
    public class EnqueueOutboxMessageCommandTest
    {
        private EnqueueOutboxMessageCommand _enqueueOutboxMessageCommand;
        private IOutboxMessageRepository _outboxMessageRepository;
        private IOutboxMessageConverter _outboxMessageConverter;
        private const string _param1 = "aString";

        [SetUp]
        public void SetUp()
        {
            _outboxMessageConverter = Substitute.For<IOutboxMessageConverter>();
            _outboxMessageRepository = Substitute.For<IOutboxMessageRepository>();
            _enqueueOutboxMessageCommand =
                new EnqueueOutboxMessageCommand(_outboxMessageConverter, _outboxMessageRepository);
        }

        [Test]
        public async Task EqneueOutboxMessage_ShouldCreateNewMessage()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            var message = new TestOutboxMessage
            {
                Param1 = _param1
            };

            _outboxMessageConverter.Serialize(message).Returns(string.Empty);
            _outboxMessageRepository.CreateAsync(Arg.Any<OutboxMessage>(), cancellationToken).Returns(ValueTask.CompletedTask);

            Act(async () => await _enqueueOutboxMessageCommand.EnqueueOutboxMessageAsync(message, cancellationToken))
                .Should().NotThrow();
            
            _outboxMessageRepository.Received(1).CreateAsync(Arg.Any<OutboxMessage>(), cancellationToken);

        }
    }
}