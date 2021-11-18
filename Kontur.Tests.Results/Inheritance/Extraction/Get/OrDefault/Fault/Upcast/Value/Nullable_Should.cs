using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Upcast.Value
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<Child?> result, StringFault? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(StringFaultResult.Succeed<Child?>(null), null);
            yield return CreateCase(StringFaultResult.Succeed<Child?>(new()), null);

            StringFault child = new("foo");
            yield return CreateCase(StringFaultResult.Fail<Child?>(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault? Process_Result(StringFaultResult<Child?> result)
        {
            return result.GetFaultOrDefault<StringFault?, Base?>();
        }
    }
}
