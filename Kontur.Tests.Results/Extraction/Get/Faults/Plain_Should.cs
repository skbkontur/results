using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Faults
{
    [TestFixture]
    internal class Plain_Should
    {
        private static TestCaseData CreateCase(Result<int> result, IEnumerable<int> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<int>.Fail(2), new[] { 2 }),
            CreateCase(Result<int>.Succeed(), Enumerable.Empty<int>()),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerated_With_Type_Safety(Result<int> result)
        {
            return result.GetFaults();
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return UpcastPlainExamples
                .Get(fault => new[] { fault }, Enumerable.Empty<Base>())
                .Select(testCase => new TestCaseData(testCase.Source).Returns(testCase.Result));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public IEnumerable<Base> Upcast(Result<Child> result)
        {
            return result.GetFaults<Base>();
        }
    }
}