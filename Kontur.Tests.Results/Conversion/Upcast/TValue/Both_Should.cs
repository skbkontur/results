using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Upcast.TValue
{
    [TestFixture]
    internal class Both_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastTValueExamples
                .GetBoth()
                .Select(example => new TestCaseData(example.Source).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Result<Base, Base> Process_Result(Result<Child, Child> result)
        {
            return result.Upcast<Base, Base>();
        }
    }
}
