using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Optional
{
    [TestFixture]
    internal class Implicit_Operator_Should
    {
        [Test]
        public void Create_Some()
        {
            const int expected = 10;

            Optional<int> optional = expected;

            var result = optional.GetValueOrThrow();
            result.Should().Be(expected);
        }

        [Test]
        public void Create_None()
        {
            Optional<int> optional = Kontur.Results.Optional.None();

            optional.HasSome.Should().BeFalse();
        }
    }
}
