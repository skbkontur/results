using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Plain.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<int?> result, int? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<int?>.Succeed(), null),
            CreateCase(Result<int?>.Fail(null), null),
            CreateCase(Result<int?>.Fail(1), 1),
        };

        [TestCaseSource(nameof(Cases))]
        public int? Process_Result(Result<int?> result)
        {
            return result.GetFaultOrDefault();
        }
    }
}
