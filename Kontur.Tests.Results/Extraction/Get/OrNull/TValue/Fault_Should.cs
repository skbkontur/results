using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrNull.TValue
{
    [TestFixture]
    internal class Fault_Should
    {
        private static TestCaseData CreateCase(Result<int, int> result, int? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<int, int>.Succeed(1), null),
            CreateCase(Result<int, int>.Fail(1), 1),
        };

        [TestCaseSource(nameof(Cases))]
        public int? Process_Result(Result<int, int> result)
        {
            return result.GetFaultOrNull();
        }
    }
}
