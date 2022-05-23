using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.TValue
{
    [TestFixture]
    internal class ResultsEnumerableEnumerable_Should
    {
        private const string Fault = "fault";
        private static readonly Result<string, int> Failure = Result<string, int>.Fail(Fault);
        private static readonly Result<string, int> Succeed4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Result<string, IEnumerable<int>> FailureEnumerable = Result<string, IEnumerable<int>>.Fail(Fault);
        private static readonly Result<string, IEnumerable<int>> SucceedEmpty = Result<string, IEnumerable<int>>.Succeed(Empty);

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            Result<string, IEnumerable<int>> expectedResult)
        {
            return new(result, enumerable1, enumerable2) { ExpectedResult = expectedResult };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Empty, Empty, FailureEnumerable),
            Create(Failure, Empty, new[] { 17 }, FailureEnumerable),
            Create(Failure, Empty, new[] { 17, 29 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, FailureEnumerable),
            Create(Succeed4, Empty, Empty, SucceedEmpty),
            Create(Succeed4, Empty, new[] { 17 }, SucceedEmpty),
            Create(Succeed4, Empty, new[] { 17, 29 }, SucceedEmpty),
            Create(Succeed4, new[] { 3 }, Empty, SucceedEmpty),
            Create(Succeed4, new[] { 3 }, new[] { 17 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24 })),
            Create(Succeed4, new[] { 3 }, new[] { 17, 29 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 36 })),
            Create(Succeed4, new[] { 3, 11 }, Empty, SucceedEmpty),
            Create(Succeed4, new[] { 3, 11 }, new[] { 17 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 32 })),
            Create(Succeed4, new[] { 3, 11 }, new[] { 17, 29 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 36, 32, 44 })),
        };

        [TestCaseSource(nameof(Cases))]
        public Result<string, IEnumerable<int>> Result_Enumerable_Enumerable(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in source
                from y in enumerable1
                from z in enumerable2
                select x + y + z;
            return result.Wrap();
        }
    }
}