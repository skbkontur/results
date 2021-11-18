using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Value
{
    [TestFixture(typeof(InvalidOperationException))]
    [TestFixture(typeof(ResultFailedException))]
    internal class Standard_Exception_Should<TException>
        where TException : Exception
    {
        [Test]
        public void Throw_If_Failure()
        {
            const string expected = "foo";
            var result = Result<string, int>.Fail(expected);

            Func<int> action = () => result.GetValueOrThrow();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
