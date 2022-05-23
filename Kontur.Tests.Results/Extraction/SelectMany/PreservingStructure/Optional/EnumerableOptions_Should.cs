using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.Optional
{
    [TestFixture]
    internal class EnumerableOptions_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly IEnumerable<Optional<int>> EmptyOptional = Enumerable.Empty<Optional<int>>();

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable,
            IEnumerable<Optional<int>> result)
        {
            return new(optional, enumerable) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, Empty, EmptyOptional),
            Create(None, new[] { 3 }, Enumerable.Repeat(None, 1)),
            Create(None, new[] { 3, 11 }, Enumerable.Repeat(None, 2)),
            Create(Some4, Empty, EmptyOptional),
            Create(Some4, new[] { 3 }, new[] { Optional<int>.Some(7),  }),
            Create(Some4, new[] { 3, 11 }, new[] { Optional<int>.Some(7), Optional<int>.Some(15) }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<Optional<int>> Enumerable_Optional(Optional<int> optional, IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in optional
                select x + y;
        }
    }
}