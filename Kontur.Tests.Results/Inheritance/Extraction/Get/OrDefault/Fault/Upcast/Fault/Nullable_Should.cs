using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Upcast.Fault
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<string> result, StringFaultBase? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(StringFaultResult.Succeed("bar"), null);

            StringFault child = new("bar");
            yield return CreateCase(StringFaultResult.Fail<string>(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFaultBase? Process_Result(StringFaultResult<string> result)
        {
            return result.GetFaultOrDefault<StringFaultBase?, string>();
        }
    }
}
