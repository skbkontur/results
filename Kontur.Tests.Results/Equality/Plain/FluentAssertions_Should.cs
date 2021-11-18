using System;
using System.Collections.Generic;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.Plain
{
    [TestFixture]
    internal class FluentAssertions_Should
    {
        private static readonly IEnumerable<TestCaseData> EqualCases = Common.CreateEqualsCases();

        [TestCaseSource(nameof(EqualCases))]
        public void Do_Not_Throw_If_Same<TFault1, TFault2>(Result<TFault1> result1, Result<TFault2> result2)
        {
            Action assertion = () => result1.Should().BeEquivalentTo(result2);

            assertion.Should().NotThrow();
        }

        private static readonly IEnumerable<TestCaseData> NotEqualCases = Common.CreateNonEqualsCases();

        [TestCaseSource(nameof(NotEqualCases))]
        public void Throw_If_Different<TFault1, TFault2>(Result<TFault1> result1, Result<TFault2> result2)
        {
            Action assertion = () => result1.Should().BeEquivalentTo(result2);

            assertion.Should().Throw<AssertionException>();
        }
    }
}