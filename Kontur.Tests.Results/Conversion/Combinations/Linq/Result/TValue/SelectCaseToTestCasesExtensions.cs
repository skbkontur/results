using System.Collections.Generic;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue
{
    internal static class SelectCaseToTestCasesExtensions
    {
        internal static IEnumerable<TestCaseData> ToTestCases(this IEnumerable<SelectCase> cases)
        {
            return cases.ToTestCases(result => result);
        }
    }
}
