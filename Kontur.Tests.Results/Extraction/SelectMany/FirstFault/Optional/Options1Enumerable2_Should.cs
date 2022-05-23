using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
#pragma warning disable S1128 // False positive. Unused "using" should be removed
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional.Using;
#pragma warning restore S1128 // Unused "using" should be removed
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional
{
    [TestFixture]
    internal class Options1Enumerable2_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Optional<IEnumerable<int>> NoneEnumerable = Optional<IEnumerable<int>>.None();
        private static readonly Optional<IEnumerable<int>> SomeEmpty = Optional<IEnumerable<int>>.Some(Empty);

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            Optional<IEnumerable<int>> result)
        {
            return new(optional, enumerable1, enumerable2) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, new[] { 3 }, new[] { 17 }, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17, 29 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, NoneEnumerable),
            Create(Some4, Empty, Empty, SomeEmpty),
            Create(Some4, Empty, new[] { 17 }, SomeEmpty),
            Create(Some4, Empty, new[] { 17, 29 }, SomeEmpty),
            Create(Some4, new[] { 3 }, Empty, SomeEmpty),
            Create(Some4, new[] { 3 }, new[] { 17 }, Optional<IEnumerable<int>>.Some(new[] { 24 })),
            Create(Some4, new[] { 3 }, new[] { 17, 29 }, Optional<IEnumerable<int>>.Some(new[] { 24, 36 })),
            Create(Some4, new[] { 3, 11 }, Empty, SomeEmpty),
            Create(Some4, new[] { 3, 11 }, new[] { 17 }, Optional<IEnumerable<int>>.Some(new[] { 24, 32 })),
            Create(Some4, new[] { 3, 11 }, new[] { 17, 29 }, Optional<IEnumerable<int>>.Some(new[] { 24, 36, 32, 44 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, NoneEnumerable),
            Create(None, Empty, new[] { 17 }, NoneEnumerable),
            Create(None, Empty, new[] { 17, 29 }, NoneEnumerable),
            Create(None, new[] { 3 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, NoneEnumerable),
        });

        [TestCaseSource(nameof(Cases1))]
        public Optional<IEnumerable<int>> Optional_Enumerable_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in optional
                from y in enumerable1
                from z in enumerable2
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, NoneEnumerable),
        });

        [TestCaseSource(nameof(Cases2))]
        public Optional<IEnumerable<int>> Enumerable_Optional_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in enumerable1
                from y in optional
                from z in enumerable2
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases3 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, SomeEmpty),
            Create(None, new[] { 3, 11 }, Empty, SomeEmpty),
        });

        [TestCaseSource(nameof(Cases3))]
        public Optional<IEnumerable<int>> Enumerable_Enumerable_Optional(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2)
        {
            var result = from x in enumerable1
                from y in enumerable2
                from z in optional
                select x + y + z;
            return result.Wrap();
        }
    }
}