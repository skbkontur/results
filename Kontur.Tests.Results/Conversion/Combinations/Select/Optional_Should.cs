using System.Globalization;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Select
{
    [TestFixture]
    internal class Optional_Should
    {
        private static TestCaseData Create(Optional<int> optional, Optional<string> result)
        {
            return new(optional) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Optional<int>.None(), Optional<string>.None()),
            Create(Optional<int>.Some(1), Optional<string>.Some("1")),
        };

        [TestCaseSource(nameof(Cases))]
        public Optional<string> Process_Value(Optional<int> optional)
        {
            return optional.Select(i => i.ToString(CultureInfo.InvariantCulture));
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<string> Process_Result(Optional<int> optional)
        {
            return optional.Select(i => Optional<string>.Some(i.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void Convert_Some_To_None()
        {
            var option = Optional<string>.Some("unused");

            var result = option.Select(_ => Optional<string>.None());

            result.Should().BeEquivalentTo(Optional<string>.None());
        }
    }
}
