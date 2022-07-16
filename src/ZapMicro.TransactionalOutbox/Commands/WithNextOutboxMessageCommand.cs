using System;
using System.Threading;
using System.Threading.Tasks;
using ZapMicro.TransactionalOutbox.Converters;
using ZapMicro.TransactionalOutbox.Messages;
using ZapMicro.TransactionalOutbox.Repositories;

namespace ZapMicro.TransactionalOutbox.Commands
{
    internal class WithNextOutboxMessageCommand: IWithNextOutboxMessageCommand
    {
        private readonly IOutboxMessageConverter _outboxMessageConverter;
        private readonly IOutboxMessageRepository _outboxMessageRepository;

        public WithNextOutboxMessageCommand(IOutboxMessageConverter outboxMessageConverter, IOutboxMessageRepository outboxMessageRepository)
        {
            _outboxMessageConverter = outboxMessageConverter;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public async ValueTask<IOutboxMessage?> WithNextIfNotNullAsync(Func<IOutboxMessage, CancellationToken, ValueTask> action, CancellationToken token)
        {
            var rawMessage = await _outboxMessageRepository.GetFirstAsync(token);
            if (rawMessage == null) return null;
            var outboxMessage = _outboxMessageConverter.Deserialize(rawMessage.Payload);
            await action(outboxMessage, token);
            _outboxMessageRepository.Delete(rawMessage);
            await _outboxMessageRepository.SaveChangesAsync(token);
            return outboxMessage;
        }
    }
}