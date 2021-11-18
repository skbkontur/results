using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.TValue.Fault.Upcast.Both
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastTValueExamples
                .GetBoth<Base?>(fault => fault, _ => null)
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Result(Result<Child, Child> result)
        {
            return result.GetFaultOrDefault<Base, Base>();
        }
    }
}
