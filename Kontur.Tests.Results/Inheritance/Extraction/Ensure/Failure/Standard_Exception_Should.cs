using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Ensure.Failure
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
            var result = StringFaultResult.Succeed(expected);

            Action action = () => result.EnsureFailure();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
