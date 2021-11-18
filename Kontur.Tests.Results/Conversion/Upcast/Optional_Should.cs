using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Upcast
{
    [TestFixture]
    internal class Optional_Should
    {
        private static IEnumerable<TestCaseData> GetCases() =>
            UpcastOptionalExamples
                .Get()
                .Select(example => new TestCaseData(example.Optional).Returns(example.Result));

        [TestCaseSource(nameof(GetCases))]
        public Optional<Base> Process_Option(Optional<Child> optional)
        {
            return optional.Upcast<Base>();
        }
    }
}
