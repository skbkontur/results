using System;
using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional
{
    [TestFixture(typeof(NoneFixtureCase))]
    [TestFixture(typeof(SomeFixtureCase))]
    [TestFixture(typeof(SomeConstantFixtureCase))]
    internal abstract class LinqTestBase<TFixtureCase, TConstantProvider, TValue>
        where TFixtureCase : IFixtureCase, new()
        where TConstantProvider : IConstantProvider<TValue>, new()
    {
        protected static readonly TValue Constant = new TConstantProvider().Get();
        protected static readonly TFixtureCase FixtureCase = new();

        private protected LinqTestBase()
        {
        }

        protected static Optional<TValue> GetOptional(TValue value) => FixtureCase.GetOptional(value, Constant);

        protected static IEnumerable<TestCaseData> CreateSelectCases(int argumentsCount, Func<int, TValue> convertValue)
        {
            return SelectCasesGenerator
                .Create(argumentsCount)
                .ToTestCases(result => result.Select(
                    value => FixtureCase.GetOptional(convertValue(value), Constant)));
        }
    }
}
