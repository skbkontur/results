using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Upcast.Value
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastExamples
                .GetValues<StringFault?>(fault => fault, _ => null)
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault? Process_Result(StringFaultResult<Child> result)
        {
            return result.GetFaultOrDefault<StringFault, Base>();
        }
    }
}
