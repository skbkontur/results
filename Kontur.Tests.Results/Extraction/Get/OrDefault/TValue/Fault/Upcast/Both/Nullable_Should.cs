using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Fault.Upcast.Both
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<Child?, Child?> result, Base? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Result<Child?, Child?>.Succeed(null), null);
            yield return CreateCase(Result<Child?, Child?>.Succeed(new()), null);
            yield return CreateCase(Result<Child?, Child?>.Fail(null), null);

            Child child = new();
            yield return CreateCase(Result<Child?, Child?>.Fail(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(Result<Child?, Child?> result)
        {
            return result.GetFaultOrDefault<Base?, Base?>();
        }
    }
}
