using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Values
{
    [TestFixture]
    internal class Optional_Should
    {
        private static TestCaseData CreateCase(Optional<int> optional, IEnumerable<int> results)
        {
            return new(optional) { ExpectedResult = results };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Optional<int>.Some(2), new[] { 2 }),
            CreateCase(Optional<int>.None(), Enumerable.Empty<int>()),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerated_With_Type_Safety(Optional<int> optional)
        {
            return optional.GetValues();
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return UpcastOptionalExamples
                .Get(Enumerable.Empty<Base>(), value => new[] { value })
                .Select(testCase => new TestCaseData(testCase.Optional).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public IEnumerable<Base> Upcast(Optional<Child> optional)
        {
            return optional.GetValues<Base>();
        }
    }
}