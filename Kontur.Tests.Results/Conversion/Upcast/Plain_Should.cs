using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Upcast
{
    [TestFixture]
    internal class Plain_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastPlainExamples
                .Get()
                .Select(example => new TestCaseData(example.Source).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Result<Base> Process_Result(Result<Child> result)
        {
            return result.Upcast<Base>();
        }
    }
}
