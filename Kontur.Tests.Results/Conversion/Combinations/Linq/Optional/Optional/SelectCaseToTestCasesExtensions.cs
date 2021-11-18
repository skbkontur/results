using System.Collections.Generic;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional
{
    internal static class SelectCaseToTestCasesExtensions
    {
        internal static IEnumerable<TestCaseData> ToTestCases(this IEnumerable<SelectCase> cases)
        {
            return cases.ToTestCases(option => option);
        }
    }
}
