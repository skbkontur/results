using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Fault.Upcast.Fault
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<Child?, int> result, Base? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Result<Child?, int>.Succeed(12), null);
            yield return CreateCase(Result<Child?, int>.Fail(null), null);

            Child child = new();
            yield return CreateCase(Result<Child?, int>.Fail(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(Result<Child?, int> result)
        {
            return result.GetFaultOrDefault<Base?, int>();
        }
    }
}
