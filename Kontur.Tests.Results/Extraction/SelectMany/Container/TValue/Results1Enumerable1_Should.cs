using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.Container.TValue
{
    [TestFixture]
    internal class Results1Enumerable1_Should
    {
        private static readonly Result<string, int> Failure = Result<string, int>.Fail("unused");
        private static readonly Result<string, int> Result4 = Result<string, int>.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Result<string, int> result,
            IEnumerable<int> enumerable,
            IEnumerable<int> values)
        {
            return new(result, enumerable) { ExpectedResult = values };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Failure, Empty, Empty),
            Create(Failure, new[] { 3 }, Empty),
            Create(Failure, new[] { 3, 11 }, Empty),
            Create(Result4, Empty, Empty),
            Create(Result4, new[] { 3 }, new[] { 7 }),
            Create(Result4, new[] { 3, 11 }, new[] { 7, 15 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Result_Enumerable(Result<string, int> result, IEnumerable<int> enumerable)
        {
            return
                from x in result
                from y in enumerable
                select x + y;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Result(Result<string, int> result, IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in result
                select x + y;
        }
    }
}