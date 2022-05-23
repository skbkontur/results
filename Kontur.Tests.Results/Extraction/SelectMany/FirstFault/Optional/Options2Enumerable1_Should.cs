using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional.Using;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional
{
    [TestFixture]
    internal class Options2Enumerable1_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly Optional<int> Some17 = Optional<int>.Some(17);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Optional<IEnumerable<int>> NoneEnumerable = Optional<IEnumerable<int>>.None();
        private static readonly Optional<IEnumerable<int>> SomeEmpty = Optional<IEnumerable<int>>.Some(Empty);

        private static TestCaseData Create(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable,
            Optional<IEnumerable<int>> result)
        {
            return new(optional1, optional2, enumerable) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, None, new[] { 3 }, NoneEnumerable),
            Create(None, None, new[] { 3, 11 }, NoneEnumerable),
            Create(None, Some17, new[] { 3 }, NoneEnumerable),
            Create(None, Some17, new[] { 3, 11 }, NoneEnumerable),
            Create(Some4, None, new[] { 3 }, NoneEnumerable),
            Create(Some4, None, new[] { 3, 11 }, NoneEnumerable),
            Create(Some4, Some17, Empty, SomeEmpty),
            Create(Some4, Some17, new[] { 3 }, Optional<IEnumerable<int>>.Some(new[] { 24 })),
            Create(Some4, Some17, new[] { 3, 11 }, Optional<IEnumerable<int>>.Some(new[] { 24, 32 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Concat(new[]
        {
            Create(None, None, Empty, NoneEnumerable),
            Create(None, Some17, Empty, NoneEnumerable),
            Create(Some4, None, Empty, NoneEnumerable),
        });

        [TestCaseSource(nameof(Cases1))]
        public Optional<IEnumerable<int>> Optional_Optional_Enumerable(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable)
        {
            var result = from x in optional1
                from y in optional2
                from z in enumerable
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Concat(new[]
        {
            Create(None, None, Empty, NoneEnumerable),
            Create(None, Some17, Empty, NoneEnumerable),
            Create(Some4, None, Empty, SomeEmpty),
        });

        [TestCaseSource(nameof(Cases2))]
        public Optional<IEnumerable<int>> Optional_Enumerable_Optional(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable)
        {
            var result = from x in optional1
                from y in enumerable
                from z in optional2
                select x + y + z;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases3 = Cases.Concat(new[]
        {
            Create(None, None, Empty, SomeEmpty),
            Create(None, Some17, Empty, SomeEmpty),
            Create(Some4, None, Empty, SomeEmpty),
        });

        [TestCaseSource(nameof(Cases3))]
        public Optional<IEnumerable<int>> Enumerable_Optional_Optional(
            Optional<int> optional1,
            Optional<int> optional2,
            IEnumerable<int> enumerable)
        {
            var result = from x in enumerable
                from y in optional1
                from z in optional2
                select x + y + z;
            return result.Wrap();
        }
    }
}