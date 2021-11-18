using System.Collections.Generic;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.Optional
{
    [TestFixture]
    internal class GetHashCode_Should
    {
        public static readonly IEnumerable<TestCaseData> Cases = Common.CreateEqualsCases();

        [TestCaseSource(nameof(Cases))]
        public void Calculate<TValue1, TValue2>(Optional<TValue1> option1, Optional<TValue2> option2)
        {
            var hashCode1 = option1.GetHashCode();
            var hashCode2 = option2.GetHashCode();

            hashCode1.Should().Be(hashCode2);
        }
    }
}