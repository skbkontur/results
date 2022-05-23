using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.Container.Optional
{
    [TestFixture]
    internal class Options2Enumerable1_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly Optional<int> Some17 = Optional<int>.Some(17);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable,
            IEnumerable<int> result)
        {
            return new(optional1, optional2, enumerable) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, None, Empty, Empty),
            Create(None, None, new[] { 3 }, Empty),
            Create(None, None, new[] { 3, 11 }, Empty),
            Create(None, Some17, Empty, Empty),
            Create(None, Some17, new[] { 3 }, Empty),
            Create(None, Some17, new[] { 3, 11 }, Empty),
            Create(Some4, None, Empty, Empty),
            Create(Some4, None, new[] { 3 }, Empty),
            Create(Some4, None, new[] { 3, 11 }, Empty),
            Create(Some4, Some17, Empty, Empty),
            Create(Some4, Some17, new[] { 3 }, new[] { 24 }),
            Create(Some4, Some17, new[] { 3, 11 }, new[] { 24, 32 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Optional_Optional_Enumerable(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable)
        {
            return
                from x in optional1
                from y in optional2
                from z in enumerable
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Optional_Enumerable_Optional(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable)
        {
            return
                from x in optional1
                from y in enumerable
                from z in optional2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Optional_Optional(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in optional1
                from z in optional2
                select x + y + z;
        }
    }
}