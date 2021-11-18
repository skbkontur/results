using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction
{
    [TestFixture]
    internal class Implicit_Operator_Bool_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<int>(new("bar")), false),
            CreateCase(StringFaultResult.Succeed(20), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Convert(StringFaultResult<int> result)
        {
            return result;
        }
    }
}