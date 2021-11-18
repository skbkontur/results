using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.TValue
{
    [TestFixture(typeof(InvalidOperationException))]
    [TestFixture(typeof(ResultFailedException))]
    internal class Standard_Exception_Should<TException>
        where TException : Exception
    {
        [Test]
        public void Throw_If_Failure()
        {
            const string expected = "bar";
            var result = Result<string, int>.Fail(expected);

            Action action = () => result.EnsureSuccess();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
