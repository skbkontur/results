using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Failure
{
    [TestFixture]
    internal class TValue_Should
    {
        private static TestCaseData Create(Result<string, int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<string, int>.Succeed(10), false),
            Create(Result<string, int>.Fail("fail"), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_Failure(Result<string, int> result)
        {
            return result.Failure;
        }
    }
}
