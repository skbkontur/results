using System.Collections.Generic;
using System.Linq;
#pragma warning disable S1128 // False positive. Unused "using" should be removed
using Kontur.Results;
#pragma warning restore S1128 // Unused "using" should be removed
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.SelectMany
{
    [TestFixture]
    internal class Results1Enumerable1_Should
    {
        private static readonly StringFaultResult<int> Failure = StringFaultResult.Fail<int>(new("unused"));
        private static readonly StringFaultResult<int> Result4 = StringFaultResult.Succeed(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            StringFaultResult<int> result,
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
        public IEnumerable<int> Result_Enumerable(StringFaultResult<int> result, IEnumerable<int> enumerable)
        {
            return
                from x in result
                from y in enumerable
                select x + y;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Result(StringFaultResult<int> result, IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in result
                select x + y;
        }
    }
}