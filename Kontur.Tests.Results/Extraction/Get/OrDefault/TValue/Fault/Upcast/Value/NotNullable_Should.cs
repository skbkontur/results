using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Fault.Upcast.Value
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastTValueExamples
                .GetValues<string?>(fault => fault, _ => null)
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Process_Result(Result<string, Child> result)
        {
            return result.GetFaultOrDefault<string, Base>();
        }
    }
}
