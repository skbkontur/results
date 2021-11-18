using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Plain
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

            Func<int> action = () => result.GetFaultOrThrow();

            action.Should().Throw<TException>();
        }
    }
}
