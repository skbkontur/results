using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Fault
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

            Func<int> action = () => result.GetFaultOrThrow();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
