using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Values.TValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Result<string, int> result, IEnumerable<int> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<string, int>.Fail("bar"), Enumerable.Empty<int>()),
            CreateCase(Result<string, int>.Succeed(2), new[] { 2 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerated_With_Type_Safety(Result<string, int> result)
        {
            return result.GetValues();
        }
    }
}