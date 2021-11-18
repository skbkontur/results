using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<int> result, int value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<int>(new("bar")), 0),
            CreateCase(StringFaultResult.Succeed(1), 1),
        };

        [TestCaseSource(nameof(Cases))]
        public int Process_Result(StringFaultResult<int> result)
        {
            return result.GetValueOrDefault();
        }
    }
}
