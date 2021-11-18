using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Success
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
            Create(Result<string, int>.Fail("fail"), false),
            Create(Result<string, int>.Succeed(10), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_HasValue(Result<string, int> result)
        {
            return result.Success;
        }
    }
}
