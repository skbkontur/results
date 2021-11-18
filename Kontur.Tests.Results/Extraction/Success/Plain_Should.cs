using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Success
{
    [TestFixture]
    internal class Plain_Should
    {
        private static TestCaseData Create(Result<int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<int>.Succeed(), true),
            Create(Result<int>.Fail(10), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_Success(Result<int> result)
        {
            return result.Success;
        }
    }
}
