using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Fault.Upcast.Value
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<string?, Child?> result, string? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Result<string?, Child?>.Succeed(null), null);
            yield return CreateCase(Result<string?, Child?>.Succeed(new()), null);
            yield return CreateCase(Result<string?, Child?>.Fail(null), null);

            const string child = "foo";
            yield return CreateCase(Result<string?, Child?>.Fail(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Process_Result(Result<string?, Child?> result)
        {
            return result.GetFaultOrDefault<string?, Base?>();
        }
    }
}
