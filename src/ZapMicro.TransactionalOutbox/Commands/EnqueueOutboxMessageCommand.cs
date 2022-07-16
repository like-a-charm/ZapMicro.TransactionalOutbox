using System;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Converters;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Messages;
using ZapMicro.TransactionalOutbox.Repositories;

namespace ZapMicro.TransactionalOutbox.Commands
{
    internal class EnqueueOutboxMessageCommand: IEnqueueOutboxMessageCommand
    {
        private readonly IOutboxMessageConverter _outboxMessageConverter;
        private readonly IOutboxMessageRepository _outboxMessageRepository;

        public EnqueueOutboxMessageCommand(IOutboxMessageConverter outboxMessageConverter, IOutboxMessageRepository outboxMessageRepository)
        {
            _outboxMessageConverter = outboxMessageConverter;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public async ValueTask EnqueueOutboxMessageAsync(IOutboxMessage message, CancellationToken token)
        {
            var rawMessagePayload = _outboxMessageConverter.Serialize(message);
            var rawMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Payload = rawMessagePayload
            };
            await _outboxMessageRepository.CreateAsync(rawMessage, token);
        }
    }
}