using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.ImplicitOperatorBool
{
    [TestFixture]
    internal class TValue_Should
    {
        private static TestCaseData CreateCase(Result<string, int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<string, int>.Fail("bar"), false),
            CreateCase(Result<string, int>.Succeed(20), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Convert(Result<string, int> result)
        {
            return result;
        }
    }
}