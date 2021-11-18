using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrNull.TValue
{
    [TestFixture]
    internal class Value_Should
    {
        private static TestCaseData CreateCase(Result<int, int> result, int? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<int, int>.Fail(1), null),
            CreateCase(Result<int, int>.Succeed(1), 1),
        };

        [TestCaseSource(nameof(Cases))]
        public int? Process_Result(Result<int, int> result)
        {
            return result.GetValueOrNull();
        }
    }
}
