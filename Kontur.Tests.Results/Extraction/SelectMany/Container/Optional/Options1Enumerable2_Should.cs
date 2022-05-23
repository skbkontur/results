using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.Container.Optional
{
    [TestFixture]
    internal class Options1Enumerable2_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> result)
        {
            return new(optional, enumerable1, enumerable2) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, Empty, Empty, Empty),
            Create(None, Empty, new[] { 17 }, Empty),
            Create(None, Empty, new[] { 17, 29 }, Empty),
            Create(None, new[] { 3 }, Empty, Empty),
            Create(None, new[] { 3 }, new[] { 17 }, Empty),
            Create(None, new[] { 3 }, new[] { 17, 29 }, Empty),
            Create(None, new[] { 3, 11 }, Empty, Empty),
            Create(None, new[] { 3, 11 }, new[] { 17 }, Empty),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, Empty),
            Create(Some4, Empty, Empty, Empty),
            Create(Some4, Empty, new[] { 17 }, Empty),
            Create(Some4, Empty, new[] { 17, 29 }, Empty),
            Create(Some4, new[] { 3 }, Empty, Empty),
            Create(Some4, new[] { 3 }, new[] { 17 }, new[] { 24 }),
            Create(Some4, new[] { 3 }, new[] { 17, 29 }, new[] { 24, 36 }),
            Create(Some4, new[] { 3, 11 }, Empty, Empty),
            Create(Some4, new[] { 3, 11 }, new[] { 17 }, new[] { 24, 32 }),
            Create(Some4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 24, 36, 32, 44 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Optional_Enumerable_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            return
                from x in optional
                from y in enumerable1
                from z in enumerable2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Optional_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            return
                from x in enumerable1
                from y in optional
                from z in enumerable2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Enumerable_Optional(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            return
                from x in enumerable1
                from y in enumerable2
                from z in optional
                select x + y + z;
        }
    }
}