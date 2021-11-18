using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Upcast.Fault
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastExamples
                .GetFaults<string?>(_ => null, value => value)
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Process_Result(StringFaultResult<string> result)
        {
            return result.GetValueOrDefault<StringFaultBase, string>();
        }
    }
}
