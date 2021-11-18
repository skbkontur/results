using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Class
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<string> result, string? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<string>(new("bar")), null),
            CreateCase(StringFaultResult.Succeed("foo"), "foo"),
        };

        [TestCaseSource(nameof(Cases))]
        public string? Process_Result(StringFaultResult<string> result)
        {
            return result.GetValueOrDefault();
        }
    }
}
