using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Plain.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<string?> result, string? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<string?>.Succeed(), null),
            CreateCase(Result<string?>.Fail(null), null),
            CreateCase(Result<string?>.Fail("foo"), "foo"),
        };

        [TestCaseSource(nameof(Cases))]
        public string? Process_Result(Result<string?> result)
        {
            return result.GetFaultOrDefault();
        }
    }
}
