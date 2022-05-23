using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Optional.Upcast
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static IEnumerable<TestCaseData> GetCases()
        {
            return UpcastOptionalExamples
                .Get<Base?>(null, value => value)
                .Select(testCase => new TestCaseData(testCase.Optional).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Optional(Optional<Child> optional)
        {
            return optional.GetValueOrDefault<Base>();
        }
    }
}
