using System.Collections.Generic;
using System.Linq;
#pragma warning disable S1128 // False positive. Unused "using" should be removed
using Kontur.Results;
#pragma warning restore S1128 // Unused "using" should be removed
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.SelectMany
{
    [TestFixture]
    internal class Results1Enumerable2_Should
    {
        private static readonly StringFaultResult<int> Failure = StringFaultResult.Fail<int>(new("unused"));
        private static readonly StringFaultResult<int> Result4 = StringFaultResult.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            StringFaultResult<int> result,
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
            StringFaultResult<int> result,
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
            StringFaultResult<int> result,
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
            StringFaultResult<int> result,
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