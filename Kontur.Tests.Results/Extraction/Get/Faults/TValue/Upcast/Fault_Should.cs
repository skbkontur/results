using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Faults.TValue.Upcast
{
    [TestFixture]
    internal class Fault_Should
    {
        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return UpcastTValueExamples
                .GetBoth(fault => new[] { fault }, _ => Enumerable.Empty<Base>())
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public IEnumerable<Base> Upcast(Result<Child, Child> result)
        {
            return result.GetFaults<Base, Child>();
        }
    }
}