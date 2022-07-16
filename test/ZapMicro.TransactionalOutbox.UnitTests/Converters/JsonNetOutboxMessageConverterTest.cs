using FluentAssertions;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Converters;

namespace ZapMicro.TransactionalOutbox.UnitTests.Converters
{
    public class JsonNetOutboxMessageConverterTest
    {
        private JsonNetOutboxMessageConverter? _jsonNetOutboxMessageConverter;

        [SetUp]
        public void SetUp()
        {
            _jsonNetOutboxMessageConverter = new JsonNetOutboxMessageConverter();
        }
        [Test]
        public void Serialize_ShouldReturnJsonString()
        {
            var message = new TestOutboxMessage
            {
                Param1 = "aString"
            };
            const string expectedJsonString = "{\"$type\":\"ZapMicro.TransactionalOutbox.UnitTests.TestOutboxMessage, ZapMicro.TransactionalOutbox.UnitTests\",\"Param1\":\"aString\"}";

            var actualJsonString = _jsonNetOutboxMessageConverter!.Serialize(message);

            actualJsonString.Should().Be(expectedJsonString);
        }
        
        [Test]
        public void Deserialize_ShouldReturnOutboxMessage()
        {
            const string jsonString = "{\"$type\":\"ZapMicro.TransactionalOutbox.UnitTests.TestOutboxMessage, ZapMicro.TransactionalOutbox.UnitTests\",\"Param1\":\"aString\"}";

            var outboxMessage = _jsonNetOutboxMessageConverter!.Deserialize(jsonString);

            outboxMessage.Should().BeOfType<TestOutboxMessage>();
            var convertersTestOutboxMessage = (TestOutboxMessage)outboxMessage;
            convertersTestOutboxMessage.Param1.Should().Be("aString");
        }
    }
}