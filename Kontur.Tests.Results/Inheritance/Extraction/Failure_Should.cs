using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction
{
    [TestFixture]
    internal class Failure_Should
    {
        private static TestCaseData Create(StringFaultResult<int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(StringFaultResult.Succeed(10), false),
            Create(StringFaultResult.Fail<int>(new("fail")), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_Failure(StringFaultResult<int> result)
        {
            return result.Failure;
        }
    }
}
