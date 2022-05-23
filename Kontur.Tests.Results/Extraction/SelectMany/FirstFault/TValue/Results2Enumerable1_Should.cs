using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue.Using;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue
{
    [TestFixture]
    internal class Results2Enumerable1_Should
    {
        private const string Fault1 = "some fault 1";
        private const string Fault2 = "some fault 2";
        private static readonly Result<string, int> Failure1 = Result<string, int>.Fail(Fault1);
        private static readonly Result<string, int> Failure2 = Result<string, int>.Fail(Fault2);
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly Result<string, int> Result17 = Result<string, int>.Succeed(17);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Result<string, IEnumerable<int>> Failure1Enumerable = Result<string, IEnumerable<int>>.Fail(Fault1);
        private static readonly Result<string, IEnumerable<int>> Failure2Enumerable = Result<string, IEnumerable<int>>.Fail(Fault2);
        private static readonly Result<string, IEnumerable<int>> SucceedEmpty = Result<string, IEnumerable<int>>.Succeed(Empty);

        private static TestCaseData Create(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable,
            Result<string, IEnumerable<int>> expectedResult)
        {
            return new(result1, result2, enumerable) { ExpectedResult = expectedResult };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure1, Failure2, new[] { 3 }, Failure1Enumerable),
            Create(Failure1, Failure2, new[] { 3, 11 }, Failure1Enumerable),
            Create(Failure1, Result17, new[] { 3 }, Failure1Enumerable),
            Create(Failure1, Result17, new[] { 3, 11 }, Failure1Enumerable),
            Create(Result4, Failure2, new[] { 3 }, Failure2Enumerable),
            Create(Result4, Failure2, new[] { 3, 11 }, Failure2Enumerable),
            Create(Result4, Result17, Empty, SucceedEmpty),
            Create(Result4, Result17, new[] { 3 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24 })),
            Create(Result4, Result17, new[] { 3, 11 }, Result<string, IEnumerable<int>>.Succeed(new[] { 24, 32 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Concat(new[]
        {
            Create(Failure1, Failure2, Empty, Failure1Enumerable),
            Create(Failure1, Result17, Empty, Failure1Enumerable),
            Create(Result4, Failure2, Empty, Failure2Enumerable),
        });

        [TestCaseSource(nameof(Cases1))]
        public Result<string, IEnumerable<int>> Result_Result_Enumerable(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable)
        {
            var result = from x in result1
                from y in result2
                from z in enumerable
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Concat(new[]
        {
            Create(Failure1, Failure2, Empty, Failure1Enumerable),
            Create(Failure1, Result17, Empty, Failure1Enumerable),
            Create(Result4, Failure2, Empty, SucceedEmpty),
        });

        [TestCaseSource(nameof(Cases2))]
        public Result<string, IEnumerable<int>> Result_Enumerable_Result(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable)
        {
            var result = from x in result1
                from y in enumerable
                from z in result2
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases3 = Cases.Concat(new[]
        {
            Create(Failure1, Failure2, Empty, SucceedEmpty),
            Create(Failure1, Result17, Empty, SucceedEmpty),
            Create(Result4, Failure2, Empty, SucceedEmpty),
        });

        [TestCaseSource(nameof(Cases3))]
        public Result<string, IEnumerable<int>> Enumerable_Result_Result(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable)
        {
            var result = from x in enumerable
                from y in result1
                from z in result2
                select x + y + z;
            return result.Wrap();
        }
    }
}