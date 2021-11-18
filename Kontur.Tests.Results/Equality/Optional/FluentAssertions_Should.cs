using System;
using System.Collections.Generic;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.Optional
{
    [TestFixture]
    internal class FluentAssertions_Should
    {
        private static readonly IEnumerable<TestCaseData> EqualCases = Common.CreateEqualsCases();

        [TestCaseSource(nameof(EqualCases))]
        public void Do_Not_Throw_If_Same<TValue1, TValue2>(Optional<TValue1> option1, Optional<TValue2> option2)
        {
            Action assertion = () => option1.Should().BeEquivalentTo(option2);

            assertion.Should().NotThrow();
        }

        private static readonly IEnumerable<TestCaseData> NotEqualCases = Common.CreateNonEqualsCases();

        [TestCaseSource(nameof(NotEqualCases))]
        public void Throw_If_Different<TValue1, TValue2>(Optional<TValue1> option1, Optional<TValue2> option2)
        {
            Action assertion = () => option1.Should().BeEquivalentTo(option2);

            assertion.Should().Throw<AssertionException>();
        }
    }
}