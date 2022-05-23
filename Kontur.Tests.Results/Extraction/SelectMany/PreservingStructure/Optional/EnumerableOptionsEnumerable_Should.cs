using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.Optional
{
    [TestFixture]
    internal class EnumerableOptionsEnumerable_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Optional<IEnumerable<int>> SomeEmpty = Optional<IEnumerable<int>>.Some(Empty);
        private static readonly Optional<IEnumerable<int>> NoneEnumerable = Optional<IEnumerable<int>>.None();
        private static readonly IEnumerable<Optional<IEnumerable<int>>> EmptyOptional = Enumerable.Empty<Optional<IEnumerable<int>>>();

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<Optional<IEnumerable<int>>> result)
        {
            return new(optional, enumerable1, enumerable2) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, Empty, Empty, EmptyOptional),
            Create(None, Empty, new[] { 17 }, EmptyOptional),
            Create(None, Empty, new[] { 17, 29 }, EmptyOptional),
            Create(None, new[] { 3 }, Empty, Enumerable.Repeat(NoneEnumerable, 1)),
            Create(None, new[] { 3 }, new[] { 17 }, Enumerable.Repeat(NoneEnumerable, 1)),
            Create(None, new[] { 3 }, new[] { 17, 29 }, Enumerable.Repeat(NoneEnumerable, 1)),
            Create(None, new[] { 3, 11 }, Empty, Enumerable.Repeat(NoneEnumerable, 2)),
            Create(None, new[] { 3, 11 }, new[] { 17 }, Enumerable.Repeat(NoneEnumerable, 2)),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, Enumerable.Repeat(NoneEnumerable, 2)),
            Create(Some4, Empty, Empty, EmptyOptional),
            Create(Some4, Empty, new[] { 17 }, EmptyOptional),
            Create(Some4, Empty, new[] { 17, 29 }, EmptyOptional),
            Create(Some4, new[] { 3 }, Empty, Enumerable.Repeat(SomeEmpty, 1)),
            Create(Some4, new[] { 3 }, new[] { 17 }, new[] { Optional<IEnumerable<int>>.Some(new[] { 24 }) }),
            Create(Some4, new[] { 3 }, new[] { 17, 29 }, new[] { Optional<IEnumerable<int>>.Some(new[] { 24, 36 }) }),
            Create(Some4, new[] { 3, 11 }, Empty, Enumerable.Repeat(SomeEmpty, 2)),
            Create(Some4, new[] { 3, 11 }, new[] { 17 }, new[] { Optional<IEnumerable<int>>.Some(new[] { 24 }), Optional<IEnumerable<int>>.Some(new[] { 32 }) }),
            Create(Some4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { Optional<IEnumerable<int>>.Some(new[] { 24, 36 }), Optional<IEnumerable<int>>.Some(new[] { 32, 44 }) }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<Optional<IEnumerable<int>>> Enumerable_Optional_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in enumerable1
                from y in optional
                from z in enumerable2
                select x + y + z;
            return result.Select(o => o.Wrap());
        }
    }
}