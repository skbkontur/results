using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Plain.Upcast
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<Child?> result, Base? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Result<Child?>.Succeed(), null);
            yield return CreateCase(Result<Child?>.Fail(null), null);

            Child child = new();
            yield return CreateCase(Result<Child?>.Fail(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(Result<Child?> result)
        {
            return result.GetFaultOrDefault<Base?>();
        }
    }
}
