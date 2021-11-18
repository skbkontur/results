using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Failure
{
    [TestFixture]
    internal class Plain_Should
    {
        private static TestCaseData Create(Result<int> result, bool failure)
        {
            return new(result) { ExpectedResult = failure };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<int>.Succeed(), false),
            Create(Result<int>.Fail(10), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_Failure(Result<int> result)
        {
            return result.Failure;
        }
    }
}
