using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.ImplicitOperatorBool
{
    [TestFixture]
    internal class Optional_Should
    {
        private static TestCaseData CreateCase(Optional<int> optional, bool hasValue)
        {
            return new(optional) { ExpectedResult = hasValue };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Optional<int>.None(), false),
            CreateCase(Optional<int>.Some(10), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Convert(Optional<int> optional)
        {
            return optional;
        }
    }
}