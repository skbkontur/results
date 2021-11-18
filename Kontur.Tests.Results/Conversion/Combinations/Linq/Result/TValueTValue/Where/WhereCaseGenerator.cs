using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue.Where
{
    internal static class WhereCaseGenerator
    {
        internal static IEnumerable<TestCaseData> CreateWhereCases<TFixtureCase>(
            this TFixtureCase fixtureCase,
            IConstantProvider<int> provider,
            int argumentsCount,
            int wherePosition)
            where TFixtureCase : IFixtureCase, new()
        {
            return SelectCasesGenerator
                .Create(argumentsCount)
                .SelectMany(testCase => CreateCases(testCase, value => fixtureCase.GetResult(value, provider), wherePosition));
        }

        private static IEnumerable<TestCaseData> CreateCases(
            SelectCase testCase,
            Func<int, Result<string, int>> resultFactory,
            int wherePosition)
        {
            var result = testCase.Result.Select(resultFactory);
            return WhereCaseFactory.Create(testCase.Args, result, wherePosition);
        }
    }
}
