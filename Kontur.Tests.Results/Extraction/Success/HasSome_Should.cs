using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Success
{
    [TestFixture]
    internal class HasSome_Should
    {
        private static TestCaseData Create(Optional<int> optional, bool hasSome)
        {
            return new(optional) { ExpectedResult = hasSome };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Optional<int>.None(), false),
            Create(Optional<int>.Some(10), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_HasValue(Optional<int> optional)
        {
            return optional.HasSome;
        }
    }
}
