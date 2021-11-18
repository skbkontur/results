using System;
using System.Collections.Generic;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain
{
    internal static class SelectCaseFactory
    {
        internal static IEnumerable<TestCaseData> GenerateCases<TFixtureCase>(
            this TFixtureCase fixtureCase,
            int argumentsCount,
            Func<int, int> convertValue)
            where TFixtureCase : IFixtureCase, new()
        {
            return SelectCasesGenerator
                .Create(argumentsCount)
                .ToTestCases(result => result.Select(
                    value => fixtureCase.GetResult(convertValue(value))));
        }

        internal static IEnumerable<TestCaseData> GenerateCases<TFixtureCase>(this TFixtureCase fixtureCase, int argumentsCount)
            where TFixtureCase : IFixtureCase, new()
        {
            return fixtureCase.GenerateCases(argumentsCount, x => x);
        }
    }
}
