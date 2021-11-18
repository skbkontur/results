using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrThrow.Fault
{
    [TestFixture(typeof(InvalidOperationException))]
    [TestFixture(typeof(ResultSucceedException))]
    internal class Standard_Exception_Should<TException>
        where TException : Exception
    {
        [Test]
        public void Throw_If_Success()
        {
            const string expected = "bar";
            var result = StringFaultResult.Succeed(expected);

            Func<StringFault> action = () => result.GetFaultOrThrow();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
