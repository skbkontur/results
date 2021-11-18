using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue.Implicit_Operator
{
    [TestFixture]
    internal class Create_Should
    {
        [Test]
        public void Create_Success()
        {
            const int expected = 10;

            Result<string, int> result = expected;

            var value = result.GetValueOrThrow();
            value.Should().Be(expected);
        }

        [Test]
        public void Create_Failure()
        {
            const int expected = 10;

            Result<int, string> result = expected;

            var fault = result.GetFaultOrThrow();
            fault.Should().Be(expected);
        }

        [Test]
        public void Select_Success()
        {
            const int expected = 10;

            Result<int, int> result = Result.Succeed(expected);

            var value = result.GetValueOrThrow();
            value.Should().Be(expected);
        }

        [Test]
        public void Select_Failure()
        {
            const int expected = 10;

            Result<int, int> result = Result.Fail(expected);

            var fault = result.GetFaultOrThrow();
            fault.Should().Be(expected);
        }
    }
}
