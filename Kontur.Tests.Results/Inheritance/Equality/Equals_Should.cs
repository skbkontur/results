using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Equality
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
        public bool Compare<TValue1, TValue2>(StringFaultResult<TValue1> result1, StringFaultResult<TValue2> result2)
        {
            return result1.Equals(result2);
        }
    }
}