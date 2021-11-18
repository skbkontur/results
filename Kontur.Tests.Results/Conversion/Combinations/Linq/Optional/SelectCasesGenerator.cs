using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional
{
    internal static class SelectCasesGenerator
    {
        internal const int InitialValue = 10;

        internal static IEnumerable<SelectCase> Create(int argumentsCount)
        {
            var terms = Enumerable.Range(InitialValue, argumentsCount).ToArray();

            return GetCases(terms, Optional<int>.Some(terms.Sum()));
        }

        private static IEnumerable<SelectCase> GetCases(
            IReadOnlyCollection<int> terms,
            Optional<int> successResult)
        {
            var boolPermutations = KPermutationOfBool.Create(terms.Count);
            return boolPermutations
                .Select(permutation => permutation
                    .Zip(terms, (success, value) => new TermInfo(success, value)))
                .Select(termInfos => CreateSelectCase(termInfos, successResult));
        }

        private static SelectCase CreateSelectCase(
            IEnumerable<TermInfo> terms,
            Optional<int> successResult)
        {
            return terms
                .Aggregate(
                    new SelectCase(Enumerable.Empty<Optional<int>>(), successResult),
                    (accumulator, term) =>
                    {
                        var (arg, newResult) = term.Success
                            ? (Optional<int>.Some(term.Value), accumulator.Result)
                            : (Optional<int>.None(), Optional<int>.None());

                        return new(accumulator.Args.Append(arg), newResult);
                    });
        }

        private record TermInfo(bool Success, int Value);
    }
}
