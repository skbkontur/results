using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.TValue
{
    [TestFixture]
    internal class Results2Enumerable1_Should
    {
        private static readonly Result<string, int> Failure = Result<string, int>.Fail("unused");
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly Result<string, int> Result17 = Result<string, int>.Succeed(17);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable,
            IEnumerable<int> values)
        {
            return new(result1, result2, enumerable) { ExpectedResult = values };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Failure, Empty, Empty),
            Create(Failure, Failure, new[] { 3 }, Empty),
            Create(Failure, Failure, new[] { 3, 11 }, Empty),
            Create(Failure, Result17, Empty, Empty),
            Create(Failure, Result17, new[] { 3 }, Empty),
            Create(Failure, Result17, new[] { 3, 11 }, Empty),
            Create(Result4, Failure, Empty, Empty),
            Create(Result4, Failure, new[] { 3 }, Empty),
            Create(Result4, Failure, new[] { 3, 11 }, Empty),
            Create(Result4, Result17, Empty, Empty),
            Create(Result4, Result17, new[] { 3 }, new[] { 24 }),
            Create(Result4, Result17, new[] { 3, 11 }, new[] { 24, 32 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Result_Result_Enumerable(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable)
        {
            return
                from x in result1
                from y in result2
                from z in enumerable
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Result_Enumerable_Result(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable)
        {
            return
                from x in result1
                from y in enumerable
                from z in result2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Result_Result(
            Result<string, int> result1,
            Result<string, int> result2,
            IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in result1
                from z in result2
                select x + y + z;
        }
    }
}