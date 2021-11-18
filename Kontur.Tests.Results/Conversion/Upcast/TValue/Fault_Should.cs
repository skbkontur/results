using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Upcast.TValue
{
    [TestFixture]
    internal class Fault_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastTValueExamples
                .GetFaults()
                .Select(example => new TestCaseData(example.Source).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Result<Base, string> Process_Result(Result<Child, string> result)
        {
            return result.Upcast<Base, string>();
        }
    }
}
