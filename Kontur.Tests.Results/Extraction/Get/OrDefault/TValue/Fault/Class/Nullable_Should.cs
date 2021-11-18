using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Fault.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<string?, string?> result, string? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<string?, string?>.Succeed(null), null),
            CreateCase(Result<string?, string?>.Succeed("foo"), null),
            CreateCase(Result<string?, string?>.Fail(null), null),
            CreateCase(Result<string?, string?>.Fail("foo"), "foo"),
        };

        [TestCaseSource(nameof(Cases))]
        public string? Process_Result(Result<string?, string?> result)
        {
            return result.GetFaultOrDefault();
        }
    }
}
