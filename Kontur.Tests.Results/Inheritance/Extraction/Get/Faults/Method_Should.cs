using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.Faults
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<int> result, IEnumerable<StringFault> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<int>(new("bar")), new StringFault[] { new("bar") }),
            CreateCase(StringFaultResult.Succeed(2), Enumerable.Empty<StringFault>()),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<StringFault> Enumerated_With_Type_Safety(StringFaultResult<int> result)
        {
            return result.GetFaults();
        }
    }
}