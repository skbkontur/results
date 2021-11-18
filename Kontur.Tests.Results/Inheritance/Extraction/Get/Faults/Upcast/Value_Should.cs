using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.Faults.Upcast
{
    [TestFixture]
    internal class Value_Should
    {
        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return UpcastExamples
                .GetBoth(fault => new[] { fault }, _ => Enumerable.Empty<StringFaultBase>())
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public IEnumerable<StringFault> Upcast(StringFaultResult<Child> result)
        {
            return result.GetFaults<StringFault, Base>();
        }
    }
}