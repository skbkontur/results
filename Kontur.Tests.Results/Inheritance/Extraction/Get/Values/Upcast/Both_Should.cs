using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.Values.Upcast
{
    [TestFixture]
    internal class Both_Should
    {
        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return UpcastExamples
                .GetBoth(_ => Enumerable.Empty<Base>(), value => new[] { value })
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public IEnumerable<Base> Upcast(StringFaultResult<Child> result)
        {
            return result.GetValues<StringFaultBase, Base>();
        }
    }
}