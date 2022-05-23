using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional.Using;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional
{
    [TestFixture]
    internal class Options1Enumerable3_Should
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
            IEnumerable<int> enumerable3,
            Optional<IEnumerable<int>> result)
        {
            return new(optional, enumerable1, enumerable2, enumerable3) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, new[] { 3 }, new[] { 17 }, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17 }, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17, 29 }, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17, 29 }, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17 }, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17 }, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100, 200 }, NoneEnumerable),
            Create(Some4, Empty, Empty, Empty, SomeEmpty),
            Create(Some4, Empty, Empty, new[] { 100 }, SomeEmpty),
            Create(Some4, Empty, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(Some4, Empty, new[] { 17 }, Empty, SomeEmpty),
            Create(Some4, Empty, new[] { 17 }, new[] { 100 }, SomeEmpty),
            Create(Some4, Empty, new[] { 17 }, new[] { 100, 200 }, SomeEmpty),
            Create(Some4, Empty, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(Some4, Empty, new[] { 17, 29 }, new[] { 100 }, SomeEmpty),
            Create(Some4, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SomeEmpty),
            Create(Some4, new[] { 3 }, Empty, Empty, SomeEmpty),
            Create(Some4, new[] { 3 }, Empty, new[] { 100 }, SomeEmpty),
            Create(Some4, new[] { 3 }, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(Some4, new[] { 3 }, new[] { 17 }, Empty, SomeEmpty),
            Create(Some4, new[] { 3 }, new[] { 17 }, new[] { 100 }, Optional<IEnumerable<int>>.Some(new[] { 124 })),
            Create(Some4, new[] { 3 }, new[] { 17 }, new[] { 100, 200 }, Optional<IEnumerable<int>>.Some(new[] { 124, 224 })),
            Create(Some4, new[] { 3 }, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(Some4, new[] { 3 }, new[] { 17, 29 }, new[] { 100 }, Optional<IEnumerable<int>>.Some(new[] { 124, 136 })),
            Create(Some4, new[] { 3 }, new[] { 17, 29 }, new[] { 100, 200 }, Optional<IEnumerable<int>>.Some(new[] { 124, 224, 136, 236 })),
            Create(Some4, new[] { 3, 11 }, Empty, Empty, SomeEmpty),
            Create(Some4, new[] { 3, 11 }, Empty, new[] { 100 }, SomeEmpty),
            Create(Some4, new[] { 3, 11 }, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(Some4, new[] { 3, 11 }, new[] { 17 }, Empty, SomeEmpty),
            Create(Some4, new[] { 3, 11 }, new[] { 17 }, new[] { 100 }, Optional<IEnumerable<int>>.Some(new[] { 124, 132 })),
            Create(Some4, new[] { 3, 11 }, new[] { 17 }, new[] { 100, 200 }, Optional<IEnumerable<int>>.Some(new[] { 124, 224, 132, 232 })),
            Create(Some4, new[] { 3, 11 }, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(Some4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100 }, Optional<IEnumerable<int>>.Some(new[] { 124, 136, 132, 144 })),
            Create(Some4, new[] { 3, 11 }, new[] { 17, 29 }, new[] { 100, 200 }, Optional<IEnumerable<int>>.Some(new[] { 124, 224, 136, 236, 132, 232, 144, 244 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, Empty, NoneEnumerable),
            Create(None, Empty, Empty, new[] { 100 }, NoneEnumerable),
            Create(None, Empty, Empty, new[] { 100, 200 }, NoneEnumerable),
            Create(None, Empty, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, Empty, new[] { 17 }, new[] { 100 }, NoneEnumerable),
            Create(None, Empty, new[] { 17 }, new[] { 100, 200 }, NoneEnumerable),
            Create(None, Empty, new[] { 17, 29 }, Empty, NoneEnumerable),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100 }, NoneEnumerable),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3 }, Empty, Empty, NoneEnumerable),
            Create(None, new[] { 3 }, Empty, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3 }, Empty, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17, 29 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, Empty, NoneEnumerable),
        });

        [TestCaseSource(nameof(Cases1))]
        public Optional<IEnumerable<int>> Optional_Enumerable_Enumerable_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in optional
                from y in enumerable1
                from z in enumerable2
                from w in enumerable3
                select x + y + z + w;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, Empty, SomeEmpty),
            Create(None, Empty, Empty, new[] { 100 }, SomeEmpty),
            Create(None, Empty, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, Empty, new[] { 17 }, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17 }, new[] { 100 }, SomeEmpty),
            Create(None, Empty, new[] { 17 }, new[] { 100, 200 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, Empty, NoneEnumerable),
            Create(None, new[] { 3 }, Empty, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3 }, Empty, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17, 29 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100, 200 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, Empty, NoneEnumerable),
        });

        [TestCaseSource(nameof(Cases2))]
        public Optional<IEnumerable<int>> Enumerable_Optional_Enumerable_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in enumerable1
                from y in optional
                from z in enumerable2
                from w in enumerable3
                select x + y + z + w;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases3 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, Empty, SomeEmpty),
            Create(None, Empty, Empty, new[] { 100 }, SomeEmpty),
            Create(None, Empty, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, Empty, new[] { 17 }, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17 }, new[] { 100 }, SomeEmpty),
            Create(None, Empty, new[] { 17 }, new[] { 100, 200 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, Empty, SomeEmpty),
            Create(None, new[] { 3 }, Empty, new[] { 100 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3 }, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, new[] { 3 }, new[] { 17, 29 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, Empty, Empty, SomeEmpty),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100 }, SomeEmpty),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3, 11 }, new[] { 17 }, Empty, NoneEnumerable),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, Empty, NoneEnumerable),
        });

        [TestCaseSource(nameof(Cases3))]
        public Optional<IEnumerable<int>> Enumerable_Enumerable_Optional_Enumerable(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in enumerable1
                from y in enumerable2
                from z in optional
                from w in enumerable3
                select x + y + z + w;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases4 = Cases.Concat(new[]
        {
            Create(None, Empty, Empty, Empty, SomeEmpty),
            Create(None, Empty, Empty, new[] { 100 }, SomeEmpty),
            Create(None, Empty, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, Empty, new[] { 17 }, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17 }, new[] { 100 }, SomeEmpty),
            Create(None, Empty, new[] { 17 }, new[] { 100, 200 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100 }, SomeEmpty),
            Create(None, Empty, new[] { 17, 29 }, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, Empty, SomeEmpty),
            Create(None, new[] { 3 }, Empty, new[] { 100 }, SomeEmpty),
            Create(None, new[] { 3 }, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3 }, new[] { 17 }, Empty, SomeEmpty),
            Create(None, new[] { 3 }, new[] { 17, 29 }, Empty, SomeEmpty),
            Create(None, new[] { 3, 11 }, Empty, Empty, SomeEmpty),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100 }, SomeEmpty),
            Create(None, new[] { 3, 11 }, Empty, new[] { 100, 200 }, SomeEmpty),
            Create(None, new[] { 3, 11 }, new[] { 17 }, Empty, SomeEmpty),
            Create(None, new[] { 3, 11 }, new[] { 17, 29 }, Empty, SomeEmpty),
        });

        [TestCaseSource(nameof(Cases4))]
        public Optional<IEnumerable<int>> Enumerable_Enumerable_Enumerable_Optional(
            Optional<int> optional,
            IEnumerable<int> enumerable1,
            IEnumerable<int> enumerable2,
            IEnumerable<int> enumerable3)
        {
            var result = from x in enumerable1
                from y in enumerable2
                from z in enumerable3
                from w in optional
                select x + y + z + w;
            return result.Wrap();
        }
    }
}