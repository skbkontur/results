using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Value
{
    [TestFixture]
    internal class Standard_Exception_Contain_Fault_Should
    {
        [Test]
        public void Throw_If_Failure()
        {
            var expected = Guid.NewGuid().ToString();
            var result = Result<string, int>.Fail(expected);

            Func<int> action = () => result.GetValueOrThrow();

            action.Should()
                .Throw<ResultFailedException<string>>()
                .WithMessage($"*{expected}*")
                .Which.Fault.Should().Be(expected);
        }
    }
}
