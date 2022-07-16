using System;
using FluentAssertions;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Entities;

namespace ZapMicro.TransactionalOutbox.UnitTests.Entities
{
    public class OutboxMessageTest
    {
        [Test]
        public void GetHashcode_ShouldReturnIdGetHashCode()
        {
            var id = Guid.Parse("1e866026-df09-4ed7-917a-aacab23dd98d");
            var outboxMessage = new OutboxMessage
            {
                Id = id,
                Payload = string.Empty,
                CreatedAt = new DateTime(2020, 1, 1)
            };
            outboxMessage.GetHashCode().Should().Be(id.GetHashCode());
        }

        [Test]
        public void Equals_ShouldReturnFalse_IfOtherObjectIsNull()
        {
            new OutboxMessage().Equals(null).Should().BeFalse();
        }
        
        [Test]
        public void Equals_ShouldReturnTrue_IfObjectsHaveSameReference()
        {
            var outboxMessage = new OutboxMessage();
            var otherMessage = outboxMessage;
            outboxMessage.Equals(otherMessage).Should().BeTrue();
        }
        
        [Test]
        public void Equals_ShouldReturnFalse_IfOtherObjectsIsOfADifferentType()
        {
            new OutboxMessage().Equals(new object()).Should().BeFalse();
        }
        
        [Test]
        public void Equals_ShouldReturnTrue_IfOutboxMessagesIdsAreEqual()
        {
            new OutboxMessage{Id = Guid.Parse("1e866026-df09-4ed7-917a-aacab23dd98d")}
                .Equals(new OutboxMessage{Id = Guid.Parse("1e866026-df09-4ed7-917a-aacab23dd98d")})
                .Should().BeTrue();
        }
    }
}