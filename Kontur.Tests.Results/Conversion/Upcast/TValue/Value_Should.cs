using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Upcast.TValue
{
    [TestFixture]
    internal class Value_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastTValueExamples
                .GetValues()
                .Select(example => new TestCaseData(example.Source).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Result<string, Base> Process_Result(Result<string, Child> result)
        {
            return result.Upcast<string, Base>();
        }
    }
}
