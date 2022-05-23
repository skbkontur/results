using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.TValue
{
    [TestFixture]
    internal class ResultsEnumerable_Should
    {
        private const string Fault = "fault";
        private static readonly Result<string, int> Failure = Result<string, int>.Fail(Fault);
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Result<string, IEnumerable<int>> FailureEnumerable = Result<string, IEnumerable<int>>.Fail(Fault);

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable,
            Result<string, IEnumerable<int>> expectedResult)
        {
            return new(result, enumerable) { ExpectedResult = expectedResult };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Empty, FailureEnumerable),
            Create(Failure, new[] { 3 }, FailureEnumerable),
            Create(Failure, new[] { 3, 11 }, FailureEnumerable),
            Create(Result4, Empty, Result<string, IEnumerable<int>>.Succeed(Empty)),
            Create(Result4, new[] { 3 }, Result<string, IEnumerable<int>>.Succeed(new[] { 7 })),
            Create(Result4, new[] { 3, 11 }, Result<string, IEnumerable<int>>.Succeed(new[] { 7, 15 })),
        };

        [TestCaseSource(nameof(Cases))]
        public Result<string, IEnumerable<int>> Result_Enumerable(Result<string, int> source, IEnumerable<int> enumerable)
        {
            var result = from x in source
                from y in enumerable
                select x + y;
            return result.Wrap();
        }
    }
}