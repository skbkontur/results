using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result
{
    internal delegate Result<string> IsSuitable(int firstValue);

    internal static class WhereCaseFactory
    {
        private const string CustomError = "custom expected error";

        internal static IEnumerable<TestCaseData> Create(
            IEnumerable<Result<string, int>> testCaseArgs,
            Result<string, int> defaultResult,
            int wherePosition)
        {
            var args = testCaseArgs.ToArray();

            var resultForFailure = Result(defaultResult, args, wherePosition);
            yield return Create(resultForFailure, args, _ => Result<string>.Fail(CustomError), "Always filter");

            yield return Create(
                resultForFailure,
                args,
                value => value == 0 ? Result<string>.Succeed() : Result<string>.Fail(CustomError),
                "Filter incorrect value");

            yield return Create(defaultResult, args, _ => Result<string>.Succeed(), "Always accept");
            yield return Create(
                defaultResult,
                args,
                value => value == SelectCasesGenerator.InitialValue ? Result<string>.Succeed() : Result<string>.Fail(CustomError),
                "Accept correct value");
        }

        private static Result<string, int> Result(
            Result<string, int> defaultResult,
            IEnumerable<Result<string, int>> args,
            int wherePosition)
        {
            var successOnlyBeforeWhere = args
                .Take(wherePosition)
                .All(arg => arg.Success);

            return successOnlyBeforeWhere
                ? Result<string, int>.Fail(CustomError)
                : defaultResult;
        }

        private static TestCaseData Create(
            Result<string, int> defaultResult,
            IReadOnlyCollection<Result<string, int>> args,
            IsSuitable isSuitable,
            string name)
        {
            var merged = string.Join(" ", args);
            return new(args.Cast<object>().Append(isSuitable).ToArray())
                {
                    ExpectedResult = defaultResult,
                    TestName = $"{name}: {merged} to {defaultResult}",
                };
        }
    }
}
