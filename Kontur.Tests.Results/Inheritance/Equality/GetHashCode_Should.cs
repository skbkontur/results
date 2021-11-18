using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Equality
{
    [TestFixture]
    internal class GetHashCode_Should
    {
        public static readonly IEnumerable<TestCaseData> Cases = Common.CreateEqualsCases();

        [TestCaseSource(nameof(Cases))]
        public void Calculate<TValue1, TValue2>(StringFaultResult<TValue1> result1, StringFaultResult<TValue2> result2)
        {
            var hashCode1 = result1.GetHashCode();
            var hashCode2 = result2.GetHashCode();

            hashCode1.Should().Be(hashCode2);
        }
    }
}