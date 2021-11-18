using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.Where
{
    internal static class WhereCaseGenerator
    {
        internal static IEnumerable<TestCaseData> Create(int argumentsCount)
        {
            return SelectCasesGenerator.Create(argumentsCount).SelectMany(CreateCases);
        }

        private static IEnumerable<TestCaseData> CreateCases(SelectCase testCase)
        {
            return WhereCaseFactory.Create(testCase.Args, testCase.Result);
        }
    }
}
