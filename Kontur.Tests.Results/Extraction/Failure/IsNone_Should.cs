using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Failure
{
    [TestFixture]
    internal class IsNone_Should
    {
        private static TestCaseData Create(Optional<int> optional, bool isNone)
        {
            return new(optional) { ExpectedResult = isNone };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Optional<int>.None(), true),
            Create(Optional<int>.Some(10), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_HasValue(Optional<int> optional)
        {
            return optional.IsNone;
        }
    }
}
