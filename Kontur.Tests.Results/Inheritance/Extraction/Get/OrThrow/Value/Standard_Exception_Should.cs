using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrThrow.Value
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
            var result = StringFaultResult.Fail<int>(new(expected));

            Func<int> action = () => result.GetValueOrThrow();

            action.Should()
                .Throw<TException>()
                .WithMessage($"*{expected}*");
        }
    }
}
