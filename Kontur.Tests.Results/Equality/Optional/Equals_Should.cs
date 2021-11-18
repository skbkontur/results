using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.Optional
{
    [TestFixture]
    internal class Equals_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases =
            from pair in new[]
                {
                    (Cases: Common.CreateNonEqualsCases(), Result: false),
                    (Cases: Common.CreateEqualsCases(), Result: true),
                }
            from testCase in pair.Cases
            select testCase.Returns(pair.Result);

        [TestCaseSource(nameof(Cases))]
        public bool Compare<TValue1, TValue2>(Optional<TValue1> option1, Optional<TValue2> option2)
        {
            return option1.Equals(option2);
        }
    }
}