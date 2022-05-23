using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue.Using;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue
{
    [TestFixture]
    internal class Results1Enumerable3_Should
    {
        private const string Fault = "some fault";
        private static readonly Result<string, int> Failure = Result<string, int>.Fail(Fault);
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Result<string, IEnumerable<int>> FailureEnumerable = Result<string, IEnumerable<int>>.Fail(Fault);
        private static readonly Result<string, IEnumerable<int>> SucceedEmpty = Result<string, IEnumerable<int>>.Succeed(Empty);

        private static TestCaseData Create(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3,
            Result<string, IEnumerable<int>> result)
        {
            return new(source, enumerable1, enumerable2, enumerable3) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, new[] { 3 }, new[] { 17 }, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17 }, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100, 200 }, FailureEnumerable),
            Create(Result4, Empty, Empty, Empty, SucceedEmpty),
            Create(Result4, Empty, Empty, new[] { 100 }, SucceedEmpty),
            Create(Result4, Empty, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Result4, Empty, new[] { 17 }, Empty, SucceedEmpty),
            Create(Result4, Empty, new[] { 17 }, new[] { 100 }, SucceedEmpty),
            Create(Result4, Empty, new[] { 17 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Result4, Empty, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Result4, Empty, new[] { 17, 29 }, new[] { 100 }, SucceedEmpty),
            Create(Result4, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Result4, new[] { 3 }, Empty, Empty, SucceedEmpty),
            Create(Result4, new[] { 3 }, Empty, new[] { 100 }, SucceedEmpty),
            Create(Result4, new[] { 3 }, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Result4, new[] { 3 }, new[] { 17 }, Empty, SucceedEmpty),
            Create(Result4, new[] { 3 }, new[] { 17 }, new[] { 100 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124 })),
            Create(Result4, new[] { 3 }, new[] { 17 }, new[] { 100, 200 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 224 })),
            Create(Result4, new[] { 3 }, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Result4, new[] { 3 }, new[] { 17, 29 }, new[] { 100 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 136 })),
            Create(Result4, new[] { 3 }, new[] { 17, 29 }, new[] { 100, 200 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 224, 136, 236 })),
            Create(Result4, new[] { 3, 11 }, Empty, Empty, SucceedEmpty),
            Create(Result4, new[] { 3, 11 }, Empty, new[] { 100 }, SucceedEmpty),
            Create(Result4, new[] { 3, 11 }, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Result4, new[] { 3, 11 }, new[] { 17 }, Empty, SucceedEmpty),
            Create(Result4, new[] { 3, 11 }, new[] { 17 }, new[] { 100 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 132 })),
            Create(Result4, new[] { 3, 11 }, new[] { 17 }, new[] { 100, 200 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 224, 132, 232 })),
            Create(Result4, new[] { 3, 11 }, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Result4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 136, 132, 144 })),
            Create(Result4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100, 200 }, Result<string, IEnumerable<int>>.Succeed(new[] { 124, 224, 136, 236, 132, 232, 144, 244 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, Empty, FailureEnumerable),
            Create(Failure, Empty, Empty, new[] { 100 }, FailureEnumerable),
            Create(Failure, Empty, Empty, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, Empty, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, Empty, new[] { 17 }, new[] { 100 }, FailureEnumerable),
            Create(Failure, Empty, new[] { 17 }, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, Empty, new[] { 17, 29 }, Empty, FailureEnumerable),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100 }, FailureEnumerable),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, Empty, FailureEnumerable),
        });

        [TestCaseSource(nameof(Cases1))]
        public Result<string, IEnumerable<int>> Result_Enumerable_Enumerable_Enumerable(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in source
                from y in enumerable1
                from z in enumerable2
                from w in enumerable3
                select x + y + z + w;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, Empty, SucceedEmpty),
            Create(Failure, Empty, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, Empty, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100, 200 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, Empty, FailureEnumerable),
        });

        [TestCaseSource(nameof(Cases2))]
        public Result<string, IEnumerable<int>> Enumerable_Result_Enumerable_Enumerable(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in enumerable1
                from y in source
                from z in enumerable2
                from w in enumerable3
                select x + y + z + w;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases3 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, Empty, SucceedEmpty),
            Create(Failure, Empty, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, Empty, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, Empty, Empty, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, Empty, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, Empty, FailureEnumerable),
        });

        [TestCaseSource(nameof(Cases3))]
        public Result<string, IEnumerable<int>> Enumerable_Enumerable_Result_Enumerable(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in enumerable1
                from y in enumerable2
                from z in source
                from w in enumerable3
                select x + y + z + w;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases4 = Cases.Concat(new[]
        {
            Create(Failure, Empty, Empty, Empty, SucceedEmpty),
            Create(Failure, Empty, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100 }, SucceedEmpty),
            Create(Failure, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, Empty, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3 }, new[] { 17 }, Empty, SucceedEmpty),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, Empty, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, Empty, Empty, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100 }, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, Empty, new[] { 100, 200 }, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, Empty, SucceedEmpty),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, Empty, SucceedEmpty),
        });

        [TestCaseSource(nameof(Cases4))]
        public Result<string, IEnumerable<int>> Enumerable_Enumerable_Enumerable_Result(
            Result<string, int> source,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in enumerable1
                from y in enumerable2
                from z in enumerable3
                from w in source
                select x + y + z + w;
            return result.Wrap();
        }
    }
}