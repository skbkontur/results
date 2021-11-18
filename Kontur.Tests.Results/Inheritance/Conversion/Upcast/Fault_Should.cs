using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Conversion.Upcast
{
    [TestFixture]
    internal class Fault_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastExamples
                .GetFaults()
                .Select(example => new TestCaseData(example.Source).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Result<StringFaultBase, string> Process_Result(StringFaultResult<string> result)
        {
            return result.Upcast<StringFaultBase, string>();
        }
    }
}
