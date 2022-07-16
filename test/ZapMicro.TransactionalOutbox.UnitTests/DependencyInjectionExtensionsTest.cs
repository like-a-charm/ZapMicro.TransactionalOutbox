using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using static ZapMicro.TransactionalOutbox.UnitTests.Utils;

namespace ZapMicro.TransactionalOutbox.UnitTests
{
    public class DependencyInjectionExtensionsTest
    {
        [Test]
        public void
            AddTransactionalOutbox_ShouldThrowArgumentNullException_IfConfigurationBuilderDirectorIsNotProvided()
        {
            var services = Substitute.For<IServiceCollection>();
            Act(() => services.AddTransactionalOutbox<TestTransactionalOutboxDbContext>(null))
                .Should().Throw<ArgumentNullException>();
        }
        
        [Test]
        public void
            AddTransactionalOutbox_ShouldNotThrow_IfConfigurationBuilderDirectorIsProvided()
        {
            var services = Substitute.For<IServiceCollection>();
            Act(() => services.AddTransactionalOutbox<TestTransactionalOutboxDbContext>(builder => builder))
                .Should().NotThrow();
        }
    }
}