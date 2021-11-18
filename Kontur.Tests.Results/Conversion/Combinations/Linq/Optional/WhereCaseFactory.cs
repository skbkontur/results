using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional
{
    internal delegate bool IsSuitable(int firstValue);

    internal static class WhereCaseFactory
    {
        internal static IEnumerable<TestCaseData> Create(IEnumerable<Optional<int>> testCaseArgs, Optional<int> result)
        {
            var args = testCaseArgs.ToArray();

            yield return Create(Optional<int>.None(), args, _ => false, "Always filter");
            yield return Create(result, args, _ => true, "Always accept");

            yield return Create(
                Optional<int>.None(),
                args,
                value => value == 0,
                "Filter incorrect value");

            yield return Create(
                result,
                args,
                value => value == SelectCasesGenerator.InitialValue,
                "Accept correct value");
        }

        private static TestCaseData Create(
            Optional<int> result,
            IReadOnlyCollection<Optional<int>> args,
            IsSuitable isSuitable,
            string name)
        {
            var @join = string.Join(" ", args);
            return new(args.Cast<object>().Append(isSuitable).ToArray())
            {
                ExpectedResult = result,
                TestName = $"{name}: {@join} to {result}",
            };
        }
    }
}
