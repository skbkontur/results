using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.TValue
{
    [TestFixture]
    internal class Results1Enumerable2_Should
    {
        private static readonly Result<string, int> Failure = Result<string, int>.Fail("unused");
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> values)
        {
            return new(result, enumerable1, enumerable2) { ExpectedResult = values };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Empty, Empty, Empty),
            Create(Failure, Empty, new[] { 17 }, Empty),
            Create(Failure, Empty, new[] { 17, 29 }, Empty),
            Create(Failure, new[] { 3 }, Empty, Empty),
            Create(Failure, new[] { 3 }, new[] { 17 }, Empty),
            Create(Failure, new[] { 3 }, new[] { 17, 29 }, Empty),
            Create(Failure, new[] { 3, 11 }, Empty, Empty),
            Create(Failure, new[] { 3, 11 }, new[] { 17 }, Empty),
            Create(Failure, new[] { 3, 11 }, new[] { 17, 29 }, Empty),
            Create(Result4, Empty, Empty, Empty),
            Create(Result4, Empty, new[] { 17 }, Empty),
            Create(Result4, Empty, new[] { 17, 29 }, Empty),
            Create(Result4, new[] { 3 }, Empty, Empty),
            Create(Result4, new[] { 3 }, new[] { 17 }, new[] { 24 }),
            Create(Result4, new[] { 3 }, new[] { 17, 29 }, new[] { 24, 36 }),
            Create(Result4, new[] { 3, 11 }, Empty, Empty),
            Create(Result4, new[] { 3, 11 }, new[] { 17 }, new[] { 24, 32 }),
            Create(Result4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 24, 36, 32, 44 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Result_Enumerable_Enumerable(
            Result<string, int> result,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            return
                from x in result
                from y in enumerable1
                from z in enumerable2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Result_Enumerable(
            Result<string, int> result,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            return
                from x in enumerable1
                from y in result
                from z in enumerable2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Enumerable_Result(
            Result<string, int> result,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            return
                from x in enumerable1
                from y in enumerable2
                from z in result
                select x + y + z;
        }
    }
}