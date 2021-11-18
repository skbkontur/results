using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.TValue
{
    [TestFixture(typeof(InvalidOperationException))]
    [TestFixture(typeof(ResultSucceedException))]
    internal class Standard_Exception_Should<TException>
        where TException : Exception
    {
        [Test]
        public void Throw_If_Success()
        {
            const string expected = "foo";
            var result = Result<int, string>.Succeed(expected);

            Action action = () => result.EnsureFailure();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
