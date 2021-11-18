using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.TValue
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
        public bool Compare<TFault1, TValue1, TFault2, TValue2>(Result<TFault1, TValue1> result1, Result<TFault2, TValue2> result2)
        {
            return result1.Equals(result2);
        }
    }
}