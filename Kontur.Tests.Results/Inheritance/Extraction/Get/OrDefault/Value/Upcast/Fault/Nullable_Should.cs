using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Upcast.Fault
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<Child?> result, Child? value)
        {
            return new(result) { ExpectedResult = value };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            Child child = new();
            yield return CreateCase(StringFaultResult.Succeed<Child?>(child), child);
            yield return CreateCase(StringFaultResult.Succeed<Child?>(null), null);

            yield return CreateCase(StringFaultResult.Fail<Child?>(new("unused")), null);
        }

        [TestCaseSource(nameof(GetCases))]
        public Child? Process_Result(StringFaultResult<Child?> result)
        {
            return result.GetValueOrDefault<StringFaultBase, Child?>();
        }
    }
}
