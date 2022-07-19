using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.BackgroundServices;
using static ZapMicro.TransactionalOutbox.UnitTests.Utils;

namespace ZapMicro.TransactionalOutbox.UnitTests.BackgroundServices
{
    public class DequeueOutboxMessagesServiceTest
    {
        internal class TestDequeueOutboxMessagesService : DequeueOutboxMessagesService
        {
            public TestDequeueOutboxMessagesService(IServiceProvider serviceProvider) : base(serviceProvider)
            {
            }

            public new virtual Task ExecuteAsync(CancellationToken token) => base.ExecuteAsync(token);

        }

        private TestDequeueOutboxMessagesService _dequeueOutboxMessagesService;
        private IDequeueOutboxMessagesServiceWorker _dequeueOutboxMessagesServiceWorker;

        [SetUp]
        public void SetUp()
        {
            var outerServiceProvider = Substitute.For<IServiceProvider>();
            var scope = Substitute.For<IServiceScope>();
            var scopedServiceProvider = Substitute.For<IServiceProvider>();
            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            _dequeueOutboxMessagesServiceWorker = Substitute.For<IDequeueOutboxMessagesServiceWorker>();

            outerServiceProvider.GetService(typeof(IServiceScopeFactory)).Returns(scopeFactory);
            scopeFactory.CreateScope().Returns(scope);
            scope.ServiceProvider.Returns(scopedServiceProvider);
            scopedServiceProvider.GetService(typeof(IDequeueOutboxMessagesServiceWorker))
                .Returns(_dequeueOutboxMessagesServiceWorker);
            
            
            _dequeueOutboxMessagesService = Substitute.ForPartsOf<TestDequeueOutboxMessagesService>(outerServiceProvider);
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldExecuteUntilCancellationIsRequested()
        {
            CancellationToken token = new CancellationTokenSource().Token;
            int calledTimes = 0;
            _dequeueOutboxMessagesService.IsCancellationRequested(Arg.Any<CancellationToken>()).Returns(info => calledTimes++ > 0);
            _dequeueOutboxMessagesServiceWorker.DequeueMessage(Arg.Any<CancellationToken>()).Returns(ValueTask.CompletedTask);
            
            _dequeueOutboxMessagesService.ClearReceivedCalls();

            Act(async () => await _dequeueOutboxMessagesService.ExecuteAsync(token))
                .Should().NotThrow();

            _dequeueOutboxMessagesServiceWorker.Received(1).DequeueMessage(token);
            _dequeueOutboxMessagesService.Received(2).IsCancellationRequested(token);
        }
        
        [Test]
        public async Task IsCancellationRequested_ShouldReturnTrue_IfCancellationTokenIsCancellationRequestedIsTrue()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            
            tokenSource.Cancel();

            _dequeueOutboxMessagesService.IsCancellationRequested(token).Should().BeTrue();
        }
        
        [Test]
        public async Task IsCancellationRequested_ShouldReturnFalse_IfCancellationTokenIsCancellationRequestedIsFalse()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            _dequeueOutboxMessagesService.IsCancellationRequested(token).Should().BeFalse();
        }
    }
}