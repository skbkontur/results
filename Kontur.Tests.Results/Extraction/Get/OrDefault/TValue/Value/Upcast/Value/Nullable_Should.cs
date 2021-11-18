using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Value.Upcast.Value
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<string?, Child?> result, Base? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Result<string?, Child?>.Fail(null), null);
            yield return CreateCase(Result<string?, Child?>.Fail("unused"), null);
            yield return CreateCase(Result<string?, Child?>.Succeed(null), null);

            Child child = new();
            yield return CreateCase(Result<string?, Child?>.Succeed(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(Result<string?, Child?> result)
        {
            return result.GetValueOrDefault<string?, Base?>();
        }
    }
}
