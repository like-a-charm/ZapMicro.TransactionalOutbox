using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Configurations;

namespace ZapMicro.TransactionalOutbox.UnitTests.Configurations
{
    public class TransactionalOutboxConfigurationBuilderTest
    {
        private TransactionalOutboxConfigurationBuilder _transactionalOutboxConfigurationBuilder;

        [SetUp]
        public void SetUp()
        {
            _transactionalOutboxConfigurationBuilder = new TransactionalOutboxConfigurationBuilder();
        }

        [Test]
        public void ConfigureDequeueOutboxMessageConfiguration_ShouldReturnSelf()
        {
            var configuration = new DequeueOutboxMessagesConfiguration();
            
            _transactionalOutboxConfigurationBuilder
                .ConfigureDequeueOutboxMessagesConfiguration(configuration)
                .Should().Be(_transactionalOutboxConfigurationBuilder);

            ((ITransactionalOutboxConfigurationBuilder)_transactionalOutboxConfigurationBuilder)
                .Build()
                .DequeueOutboxMessagesConfiguration
                .Should().Be(configuration);
        }
        
        [Test]
        public void ConfigureOutboxMessageHandler_ShouldReturnSelf()
        {
            _transactionalOutboxConfigurationBuilder
                .ConfigureOutboxMessageHandler<TestOutboxMessageHandler, TestOutboxMessage>()
                .Should().Be(_transactionalOutboxConfigurationBuilder);

            ((ITransactionalOutboxConfigurationBuilder)_transactionalOutboxConfigurationBuilder)
                .Build()
                .OutboxMessageHandlersFactories
                .Count()
                .Should().Be(1);
        }

    }
}