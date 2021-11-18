using System.Collections.Generic;
using System.Linq;
#pragma warning disable S1128 // False positive. Unused "using" should be removed
using Kontur.Results;
#pragma warning restore S1128 // Unused "using" should be removed
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.SelectMany
{
    [TestFixture]
    internal class Results2Enumerable1_Should
    {
        private static readonly StringFaultResult<int> Failure = StringFaultResult.Fail<int>(new("unused"));
        private static readonly StringFaultResult<int> Result4 = StringFaultResult.Succeed(4);
        private static readonly StringFaultResult<int> Result17 = StringFaultResult.Succeed(17);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            StringFaultResult<int> result1,
            StringFaultResult<int> result2,
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
            StringFaultResult<int> result1,
            StringFaultResult<int> result2,
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
            StringFaultResult<int> result1,
            StringFaultResult<int> result2,
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
            StringFaultResult<int> result1,
            StringFaultResult<int> result2,
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