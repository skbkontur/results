using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<int> result, StringFault? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(StringFaultResult.Succeed(1), null);

            StringFault fault = new("bar");
            yield return CreateCase(StringFaultResult.Fail<int>(fault), fault);
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault? Process_Result(StringFaultResult<int> result)
        {
            return result.GetFaultOrDefault();
        }
    }
}
