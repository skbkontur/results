using System.Collections.Generic;
using System.Globalization;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.Select.DifferentTypes
{
    internal static class SelectCaseToDifferentTypeTestCasesExtensions
    {
        internal static IEnumerable<TestCaseData> ToDifferentTypeTestCases(this IEnumerable<SelectCase> cases)
        {
            return cases.ToTestCases(result => result.MapValue(ConvertToString));
        }

        internal static string ConvertToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
