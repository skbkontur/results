using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue.Using;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue
{
    [TestFixture]
    internal class Results1Enumerable2_Should
    {
        private const string Fault = "some fault";
        private static readonly Result<string, int> Failure = Result<string, int>.Fail(Fault);
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
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
            Create(Failure, new[] { 3 }, new[] { 17 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, FailureEnumerable),
            Create(Result4, Empty, Empty, SucceedEmpty),
            Create(Result4, Empty, new[] { 17 }, SucceedEmpty),
            Create(Result4, Empty, new[] { 17, 29 }, SucceedEmpty),
            Create(Result4, new[] { 3 }, Empty, SucceedEmpty),
            Create(Result4, new[] { 3 }, new[] { 17 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24 })),
            Create(Result4, new[] { 3 }, new[] { 17, 29 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 36 })),
            Create(Result4, new[] { 3, 11 }, Empty, SucceedEmpty),
            Create(Result4, new[] { 3, 11 }, new[] { 17 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 32 })),
            Create(Result4, new[] { 3, 11 }, new[] { 17, 29 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 36, 32, 44 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, FailureEnumerable),
            Create(Failure, Empty, new[] { 17 }, FailureEnumerable),
            Create(Failure, Empty, new[] { 17, 29 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, FailureEnumerable),
        });

        [TestCaseSource(nameof(Cases1))]
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

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, FailureEnumerable),
        });

        [TestCaseSource(nameof(Cases2))]
        public Result<string, IEnumerable<int>> Enumerable_Result_Enumerable(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in enumerable1
                from y in source
                from z in enumerable2
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases3 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, Empty, SucceedEmpty),
        });

        [TestCaseSource(nameof(Cases3))]
        public Result<string, IEnumerable<int>> Enumerable_Enumerable_Result(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in enumerable1
                from y in enumerable2
                from z in source
                select x + y + z;
            return result.Wrap();
        }
    }
}