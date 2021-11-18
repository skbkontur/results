using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Class
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<string> result, StringFault? fault)
        {
            return new(result) { ExpectedResult = fault };
        }

        private static IEnumerable<TestCaseData> Cases()
        {
            yield return CreateCase(StringFaultResult.Succeed("bar"), null);

            StringFault fault = new("foo");
            yield return CreateCase(StringFaultResult.Fail<string>(fault), fault);
        }

        [TestCaseSource(nameof(Cases))]
        public StringFault? Process_Result(StringFaultResult<string> result)
        {
            return result.GetFaultOrDefault();
        }
    }
}
