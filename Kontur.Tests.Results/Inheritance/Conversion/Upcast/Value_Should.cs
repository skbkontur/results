using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Conversion.Upcast
{
    [TestFixture]
    internal class Value_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastExamples
                .GetValues()
                .Select(example => new TestCaseData(example.Source).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Result<StringFault, Base> Process_Result(StringFaultResult<Child> result)
        {
            return result.Upcast<StringFault, Base>();
        }
    }
}
