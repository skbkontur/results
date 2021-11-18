using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Faults.TValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Result<string, int> result, IEnumerable<string> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<string, int>.Fail("bar"), new[] { "bar" }),
            CreateCase(Result<string, int>.Succeed(2), Enumerable.Empty<string>()),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<string> Enumerated_With_Type_Safety(Result<string, int> result)
        {
            return result.GetFaults();
        }
    }
}