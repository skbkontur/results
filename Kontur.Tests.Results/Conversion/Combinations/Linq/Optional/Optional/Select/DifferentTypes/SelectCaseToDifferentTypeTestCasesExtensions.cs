using System.Collections.Generic;
using System.Globalization;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.Select.DifferentTypes
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
