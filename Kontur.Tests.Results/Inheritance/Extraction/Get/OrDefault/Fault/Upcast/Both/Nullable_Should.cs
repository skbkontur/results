using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Upcast.Both
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<Child?> result, StringFaultBase? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(StringFaultResult.Succeed<Child?>(null), null);
            yield return CreateCase(StringFaultResult.Succeed<Child?>(new()), null);

            StringFault fault = new("bar");
            yield return CreateCase(StringFaultResult.Fail<Child?>(fault), fault);
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFaultBase? Process_Result(StringFaultResult<Child?> result)
        {
            return result.GetFaultOrDefault<StringFault?, Base?>();
        }
    }
}
