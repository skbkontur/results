using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.Values
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<int> result, IEnumerable<int> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<int>(new("bar")), Enumerable.Empty<int>()),
            CreateCase(StringFaultResult.Succeed(2), new[] { 2 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerated_With_Type_Safety(StringFaultResult<int> result)
        {
            return result.GetValues();
        }
    }
}