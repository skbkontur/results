using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain
{
    [TestFixture]
    internal class Implicit_Operator_Should
    {
        [Test]
        public void Create_Failure()
        {
            const int expected = 10;

            Result<int> result = expected;

            var fault = result.GetFaultOrThrow();
            fault.Should().Be(expected);
        }

        [Test]
        public void Create_Success()
        {
            Result<int> result = Result.Succeed();

            result.Success.Should().BeTrue();
        }
    }
}
