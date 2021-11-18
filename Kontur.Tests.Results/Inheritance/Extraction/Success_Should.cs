using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction
{
    [TestFixture]
    internal class Success_Should
    {
        private static TestCaseData Create(StringFaultResult<int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(StringFaultResult.Fail<int>(new("fail")), false),
            Create(StringFaultResult.Succeed(10), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Pass_HasValue(StringFaultResult<int> result)
        {
            return result.Success;
        }
    }
}
