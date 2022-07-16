using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Entities;
using ZapMicro.TransactionalOutbox.Repositories;
using static ZapMicro.TransactionalOutbox.UnitTests.Utils;

namespace ZapMicro.TransactionalOutbox.UnitTests.Repositories
{
    public class OutboxMessageRepositoryTest
    {
        private OutboxMessageRepository<TestTransactionalOutboxDbContext>? _outboxMessageRepository;
        private TestTransactionalOutboxDbContext _transactionalOutboxDbContext;

        [SetUp]
        public void SetUp()
        {
            _transactionalOutboxDbContext = Substitute.ForPartsOf<TestTransactionalOutboxDbContext>();
            _outboxMessageRepository = new OutboxMessageRepository<TestTransactionalOutboxDbContext>(_transactionalOutboxDbContext);
            _transactionalOutboxDbContext.OutboxMessages.RemoveRange(_transactionalOutboxDbContext.OutboxMessages.ToList());
            _transactionalOutboxDbContext.SaveChangesAsync();
        }

        [Test]
        public async Task CreateAsync_ShouldCreateMessage_IfMessageIsNotNull()
        {
            var outboxMessage = new OutboxMessage
            {
                CreatedAt = new DateTime(2020, 1, 1),
                Id = Guid.Empty,
                Payload = string.Empty
            };
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            var dbSet = Substitute.For<DbSet<OutboxMessage>>();
            _transactionalOutboxDbContext.OutboxMessages.Returns(dbSet);
            var addAsyncResult = ValueTask.FromResult(default(EntityEntry<OutboxMessage>));
            dbSet.AddAsync(outboxMessage, cancellationToken).Returns(addAsyncResult!);

            Act(async () => await _outboxMessageRepository!
                    .CreateAsync(outboxMessage, cancellationToken).AsTask())
                .Should().NotThrow();
            
            dbSet.Received(1).AddAsync(outboxMessage, cancellationToken);
        }
        
        [Test]
        public async Task CreateAsync_ShouldThrowArgumentNullException_IfMessageIsNull()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            Act(async () =>await _outboxMessageRepository!
                    .CreateAsync(null, cancellationToken).AsTask())
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task GetFirstAsync_ShouldReturnOldestMessage_IfOutboxIsNotEmpty()
        {
            var oldestOutboxMessage = new OutboxMessage
            {
                CreatedAt = new DateTime(2020, 1, 1),
                Id = Guid.NewGuid(),
                Payload = string.Empty
            };
            var newestOutboxMessage = new OutboxMessage
            {
                CreatedAt = new DateTime(2020, 1, 2),
                Id = Guid.NewGuid(),
                Payload = string.Empty
            };

            _transactionalOutboxDbContext.OutboxMessages.Add(newestOutboxMessage);
            _transactionalOutboxDbContext.OutboxMessages.Add(oldestOutboxMessage);
            await _transactionalOutboxDbContext.SaveChangesAsync();
            
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var actualOutboxMessage = await _outboxMessageRepository!.GetFirstAsync(cancellationToken);

            actualOutboxMessage.Should().Be(oldestOutboxMessage);
        }
        
        [Test]
        public async Task GetFirstAsync_ShouldReturnOldestMessageNull_IfOutboxIsEmpty()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var actualOutboxMessage = await _outboxMessageRepository!.GetFirstAsync(cancellationToken);

            actualOutboxMessage.Should().BeNull();
        }

        
        [Test]
        public void Delete_ShouldCreateMessage_IfMessageIsNotNull()
        {
            var outboxMessage = new OutboxMessage
            {
                CreatedAt = new DateTime(2020, 1, 1),
                Id = Guid.Empty,
                Payload = string.Empty
            };

            var dbSet = Substitute.For<DbSet<OutboxMessage>>();
            _transactionalOutboxDbContext.OutboxMessages.Returns(dbSet);

            Act(() => _outboxMessageRepository!
                    .Delete(outboxMessage))
                .Should().NotThrow();
            
            dbSet.Received(1).Remove(outboxMessage);
        }
        
        [Test]
        public void Delete_ShouldThrowArgumentNullException_IfMessageIsNull()
        {
            Act(() => _outboxMessageRepository!
                    .Delete(null))
                .Should().Throw<ArgumentNullException>();
            
        }

        [Test]
        public async Task SavesChangesAsync_ShouldInvokeDbContextSaveChangesAsync()
        {
            _transactionalOutboxDbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
                .Returns(info => Task.FromResult(1));
            _transactionalOutboxDbContext.ClearReceivedCalls();
            await _outboxMessageRepository!.SaveChangesAsync(new CancellationToken());
            _transactionalOutboxDbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        }
    }
}