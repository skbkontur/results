using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Fault.Upcast.Both
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastExamples
                .GetBoth<StringFaultBase?>(fault => fault, _ => null)
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFaultBase? Process_Result(StringFaultResult<Child> result)
        {
            return result.GetFaultOrDefault<StringFaultBase, Base>();
        }
    }
}
