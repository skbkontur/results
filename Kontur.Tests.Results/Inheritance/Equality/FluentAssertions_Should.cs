using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Equality
{
    [TestFixture]
    internal class FluentAssertions_Should
    {
        private static readonly IEnumerable<TestCaseData> EqualCases = Common.CreateEqualsCases();

        [TestCaseSource(nameof(EqualCases))]
        public void Do_Not_Throw_If_Same<TValue1, TValue2>(StringFaultResult<TValue1> result1, StringFaultResult<TValue2> result2)
        {
            Action assertion = () => result1.Should().BeEquivalentTo(result2);

            assertion.Should().NotThrow();
        }

        private static readonly IEnumerable<TestCaseData> NotEqualCases = Common.CreateNonEqualsCases();

        [TestCaseSource(nameof(NotEqualCases))]
        public void Throw_If_Different<TValue1, TValue2>(StringFaultResult<TValue1> result1, StringFaultResult<TValue2> result2)
        {
            Action assertion = () => result1.Should().BeEquivalentTo(result2);

            assertion.Should().Throw<AssertionException>();
        }
    }
}