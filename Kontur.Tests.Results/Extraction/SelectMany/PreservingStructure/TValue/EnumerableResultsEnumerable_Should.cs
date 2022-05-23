using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.TValue
{
    [TestFixture]
    internal class EnumerableResultsEnumerable_Should
    {
        private const string Fault = "fault";
        private static readonly Result<string, int> Failure = Result<string, int>.Fail(Fault);
        private static readonly Result<string, int> Succeed4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Result<string, IEnumerable<int>> SucceedEmpty = Result<string, IEnumerable<int>>.Succeed(Empty);
        private static readonly Result<string, IEnumerable<int>> FailureEnumerable = Result<string, IEnumerable<int>>.Fail(Fault);
        private static readonly IEnumerable<Result<string, IEnumerable<int>>> EmptyResult = Enumerable.Empty<Result<string, IEnumerable<int>>>();

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<Result<string, IEnumerable<int>>> expectedResult)
        {
            return new(result, enumerable1, enumerable2) { ExpectedResult = expectedResult };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Empty, Empty, EmptyResult),
            Create(Failure, Empty, new[] { 17 }, EmptyResult),
            Create(Failure, Empty, new[] { 17, 29 }, EmptyResult),
            Create(Failure, new[] { 3 }, Empty, Enumerable.Repeat(FailureEnumerable, 1)),
            Create(Failure, new[] { 3 }, new[] { 17 }, Enumerable.Repeat(FailureEnumerable, 1)),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, Enumerable.Repeat(FailureEnumerable, 1)),
            Create(Failure, new[] { 3, 11 }, Empty, Enumerable.Repeat(FailureEnumerable, 2)),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, Enumerable.Repeat(FailureEnumerable, 2)),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, Enumerable.Repeat(FailureEnumerable, 2)),
            Create(Succeed4, Empty, Empty, EmptyResult),
            Create(Succeed4, Empty, new[] { 17 }, EmptyResult),
            Create(Succeed4, Empty, new[] { 17, 29 }, EmptyResult),
            Create(Succeed4, new[] { 3 }, Empty, Enumerable.Repeat(SucceedEmpty, 1)),
            Create(Succeed4, new[] { 3 }, new[] { 17 }, new[] { Result<string, IEnumerable<int>>.Succeed(new[] { 24 }) }),
            Create(Succeed4, new[] { 3 }, new[] { 17, 29 }, new[] { Result<string, IEnumerable<int>>.Succeed(new[] { 24, 36 }) }),
            Create(Succeed4, new[] { 3, 11 }, Empty, Enumerable.Repeat(SucceedEmpty, 2)),
            Create(Succeed4, new[] { 3, 11 }, new[] { 17 }, new[] { Result<string, IEnumerable<int>>.Succeed(new[] { 24 }), Result<string, IEnumerable<int>>.Succeed(new[] { 32 }) }),
            Create(Succeed4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { Result<string, IEnumerable<int>>.Succeed(new[] { 24, 36 }), Result<string, IEnumerable<int>>.Succeed(new[] { 32, 44 }) }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<Result<string, IEnumerable<int>>> Enumerable_Result_Enumerable(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in enumerable1
                from y in source
                from z in enumerable2
                select x + y + z;
            return result.Select(o => o.Wrap());
        }
    }
}