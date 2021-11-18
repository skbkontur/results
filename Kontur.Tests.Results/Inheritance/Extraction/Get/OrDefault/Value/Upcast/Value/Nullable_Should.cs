using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Upcast.Value
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<Child?> result, Base? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(StringFaultResult.Fail<Child?>(new("unused")), null);
            yield return CreateCase(StringFaultResult.Succeed<Child?>(null), null);

            Child child = new();
            yield return CreateCase(StringFaultResult.Succeed<Child?>(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(StringFaultResult<Child?> result)
        {
            return result.GetValueOrDefault<StringFault, Base?>();
        }
    }
}
