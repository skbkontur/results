using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.TValue
{
    [TestFixture]
    internal class EnumerableResults_Should
    {
        private static readonly Result<string, int> Failure = Result<string, int>.Fail("unused");
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly IEnumerable<Result<string, int>> EnumerableResult = Enumerable.Empty<Result<string, int>>();

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable,
            IEnumerable<Result<string, int>> expectedResult)
        {
            return new(result, enumerable) { ExpectedResult = expectedResult };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Empty, EnumerableResult),
            Create(Failure, new[] { 3 }, Enumerable.Repeat(Failure, 1)),
            Create(Failure, new[] { 3, 11 }, Enumerable.Repeat(Failure, 2)),
            Create(Result4, Empty, EnumerableResult),
            Create(Result4, new[] { 3 }, new[] { Result<string, int>.Succeed(7) }),
            Create(Result4, new[] { 3, 11 }, new[] { Result<string, int>.Succeed(7), Result<string, int>.Succeed(15) }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<Result<string, int>> Enumerable_Result(Result<string, int> result, IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in result
                select x + y;
        }
    }
}