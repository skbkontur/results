using System.Collections.Generic;
using System.Linq;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.Where
{
    internal static class WhereCaseGenerator
    {
        internal static IEnumerable<TestCaseData> Create(int argumentsCount, int wherePosition)
        {
            return SelectCasesGenerator
                .Create(argumentsCount)
                .SelectMany(testCase => CreateCases(testCase, wherePosition));
        }

        private static IEnumerable<TestCaseData> CreateCases(SelectCase testCase, int wherePosition)
        {
            return WhereCaseFactory.Create(testCase.Args, testCase.Result, wherePosition);
        }
    }
}
