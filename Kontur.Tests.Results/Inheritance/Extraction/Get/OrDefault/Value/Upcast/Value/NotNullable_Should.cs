using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrDefault.Value.Upcast.Value
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastExamples
                .GetValues<Base?>(_ => null, value => value)
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(StringFaultResult<Child> result)
        {
            return result.GetValueOrDefault<StringFault, Base>();
        }
    }
}
