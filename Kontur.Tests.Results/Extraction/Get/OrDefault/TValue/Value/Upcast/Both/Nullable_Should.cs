using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Value.Upcast.Both
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Result<Child?, Child?> result, Base? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Result<Child?, Child?>.Fail(null), null);
            yield return CreateCase(Result<Child?, Child?>.Fail(new()), null);
            yield return CreateCase(Result<Child?, Child?>.Succeed(null), null);

            Child child = new();
            yield return CreateCase(Result<Child?, Child?>.Succeed(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(Result<Child?, Child?> result)
        {
            return result.GetValueOrDefault<Base?, Base?>();
        }
    }
}
