using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Values.TValue.Upcast
{
    [TestFixture]
    internal class Both_Should
    {
        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return UpcastTValueExamples
                .GetBoth(_ => Enumerable.Empty<Base>(), value => new[] { value })
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public IEnumerable<Base> Upcast(Result<Child, Child> result)
        {
            return result.GetValues<Base, Base>();
        }
    }
}