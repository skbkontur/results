using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.Plain
{
    [TestFixture(typeof(InvalidOperationException))]
    [TestFixture(typeof(ResultSucceedException))]
    internal class Standard_Exception_Should<TException>
        where TException : Exception
    {
        [Test]
        public void Throw_If_Success()
        {
            var result = Result<int>.Succeed();

            Action action = () => result.EnsureFailure();

            action.Should().Throw<TException>();
        }
    }
}
