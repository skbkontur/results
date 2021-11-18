using System;
using System.Collections.Generic;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue
{
    [TestFixture(typeof(FailureConstantFixtureCase))]
    [TestFixture(typeof(FailureVariableFixtureCase))]
    [TestFixture(typeof(SuccessConstantFixtureCase))]
    [TestFixture(typeof(SuccessVariableFixtureCase))]
    internal abstract class LinqTestBase<TFixtureCase, TConstantProvider, TValue>
        where TFixtureCase : IFixtureCase, new()
        where TConstantProvider : IConstantProvider<TValue>, new()
    {
        protected static readonly TConstantProvider Provider = new();
        protected static readonly TFixtureCase FixtureCase = new();

        private protected LinqTestBase()
        {
        }

        protected static Result<string, TValue> GetResult(TValue value) => FixtureCase.GetResult(value, Provider);

        protected static IEnumerable<TestCaseData> CreateSelectCases(int argumentsCount, Func<int, TValue> convertValue)
        {
            return SelectCasesGenerator
                .Create(argumentsCount)
                .ToTestCases(result => result.Select(
                    value => FixtureCase.GetResult(convertValue(value), Provider)));
        }
    }
}
