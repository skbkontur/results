using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional
{
    internal static class SelectCaseToTestCasesExtensions
    {
        internal static IEnumerable<TestCaseData> ToTestCases<T>(
            this IEnumerable<SelectCase> cases,
            Func<Optional<int>, Optional<T>> resultSelector)
        {
            return cases.Select(
                testCase => new TestCaseData(testCase.Args.Cast<object>().ToArray())
                    .Returns(resultSelector(testCase.Result)));
        }
    }
}
