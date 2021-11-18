using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result
{
    internal static class SelectCaseToTestCasesExtensions
    {
        internal static IEnumerable<TestCaseData> ToTestCases<TResult>(
            this IEnumerable<SelectCase> cases,
            Func<Result<string, int>, TResult> resultSelector)
        {
            return cases.Select(
                testCase => new TestCaseData(testCase.Args.Cast<object>().ToArray())
                    .Returns(resultSelector(testCase.Result)));
        }
    }
}
