using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Value.Upcast.Fault
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<Child?, int> result, int value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            const int value = 777;
            yield return CreateCase(Result<Child?, int>.Succeed(value), value);
            yield return CreateCase(Result<Child?, int>.Fail(null), 0);

            yield return CreateCase(Result<Child?, int>.Fail(new()), 0);
        }

        [TestCaseSource(nameof(GetCases))]
        public int Process_Result(Result<Child?, int> result)
        {
            return result.GetValueOrDefault<Base?, int>();
        }
    }
}
