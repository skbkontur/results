using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<string?> result, string? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<string?>(new("foo")), null),
            CreateCase(StringFaultResult.Succeed<string?>(null), null),
            CreateCase(StringFaultResult.Succeed<string?>("foo"), "foo"),
        };

        [TestCaseSource(nameof(Cases))]
        public string? Process_Result(StringFaultResult<string?> result)
        {
            return result.GetValueOrDefault();
        }
    }
}
