using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.TValue
{
    [TestFixture]
    internal class Standard_Exception_Contain_Value_Should
    {
        [Test]
        public void Throw_If_Success()
        {
            var expected = Guid.NewGuid().ToString();
            var result = Result<int, string>.Succeed(expected);

            Action action = () => result.EnsureFailure();

            action.Should()
                .Throw<ResultSucceedException<string>>()
                .WithMessage($"*{expected}*")
                .Which.Value.Should().Be(expected);
        }
    }
}
