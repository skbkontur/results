using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators
{
    internal static class SelectCasesGenerator
    {
        internal const int InitialValue = 10;

        internal static IEnumerable<SelectCase> Create(int argumentsCount)
        {
            var terms = Enumerable.Range(InitialValue, argumentsCount).ToArray();

            return GetCases(terms, Result<string, int>.Succeed(terms.Sum()));
        }

        private static IEnumerable<SelectCase> GetCases(
            IReadOnlyCollection<int> terms,
            Result<string, int> successResult)
        {
            var boolPermutations = KPermutationOfBool.Create(terms.Count);
            return boolPermutations
                .Select(permutation => permutation
                    .Zip(terms, (success, value) => new TermInfo(success, value)))
                .Select(termInfos => CreateSelectCase(termInfos, successResult));
        }

        private static SelectCase CreateSelectCase(
            IEnumerable<TermInfo> terms,
            Result<string, int> successResult)
        {
            return terms
                .Aggregate(
                    new SelectCase(Enumerable.Empty<Result<string, int>>(), successResult),
                    (accumulator, term) =>
                    {
                        var (isSuccess, value) = term;

                        var (arg, newResult) = isSuccess
                            ? (Result<string, int>.Succeed(value), accumulator.Result)
                            : (CreateFailure(value), accumulator.Result.Select(_ => CreateFailure(value)));

                        return new(accumulator.Args.Append(arg), newResult);
                    });
        }

        private static Result<string, int> CreateFailure(int value)
        {
            var message = "foo-" + value;
            return Result<string, int>.Fail(message);
        }

        private record TermInfo(bool Success, int Value);
    }
}
