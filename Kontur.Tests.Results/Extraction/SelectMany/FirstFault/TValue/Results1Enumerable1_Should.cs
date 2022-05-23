using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
#pragma warning disable S1128 // False positive. Unused "using" should be removed
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue.Using;
#pragma warning restore S1128 // Unused "using" should be removed
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue
{
    [TestFixture]
    internal class Results1Enumerable1_Should
    {
        private const string Fault = "some fault";
        private static readonly Result<string, int> Failure = Result<string, int>.Fail(Fault);
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Result<string, IEnumerable<int>> FailureEnumerable = Result<string, IEnumerable<int>>.Fail(Fault);
        private static readonly Result<string, IEnumerable<int>> SucceedEmpty = Result<string, IEnumerable<int>>.Succeed(Empty);

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable,
            Result<string, IEnumerable<int>> expectedResult)
        {
            return new(result, enumerable) { ExpectedResult = expectedResult };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, new[] { 3 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, FailureEnumerable),
            Create(Result4, Empty, SucceedEmpty),
            Create(Result4, new[] { 3 }, Result<string, IEnumerable<int>>.Succeed(new[] { 7 })),
            Create(Result4, new[] { 3, 11 }, Result<string, IEnumerable<int>>.Succeed(new[] { 7, 15 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Append(Create(Failure, Empty, FailureEnumerable));

        [TestCaseSource(nameof(Cases1))]
        public Result<string, IEnumerable<int>> Result_Enumerable(Result<string, int> source, IEnumerable<int> enumerable)
        {
            var result = from x in source
                from y in enumerable
                select x + y;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Append(Create(Failure, Empty, SucceedEmpty));

        [TestCaseSource(nameof(Cases2))]
        public Result<string, IEnumerable<int>> Enumerable_Result(Result<string, int> source, IEnumerable<int> enumerable)
        {
            var result = from x in enumerable
                from y in source
                select x + y;
            return result.Wrap();
        }
    }
}