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
    internal class Options1Enumerable1_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Optional<IEnumerable<int>> NoneEnumerable = Optional<IEnumerable<int>>.None();
        private static readonly Optional<IEnumerable<int>> SomeEmpty = Optional<IEnumerable<int>>.Some(Empty);

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable,
            Optional<IEnumerable<int>> result)
        {
            return new(optional, enumerable) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, new[] { 3 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, NoneEnumerable),
            Create(Some4, Empty, SomeEmpty),
            Create(Some4, new[] { 3 }, Optional<IEnumerable<int>>.Some(new[] { 7 })),
            Create(Some4, new[] { 3, 11 }, Optional<IEnumerable<int>>.Some(new[] { 7, 15 })),
        };

        private static readonly IEnumerable<TestCaseData> Cases1 = Cases.Append(Create(None, Empty, NoneEnumerable));

        [TestCaseSource(nameof(Cases1))]
        public Optional<IEnumerable<int>> Optional_Enumerable(Optional<int> optional, IEnumerable<int> enumerable)
        {
            var result = from x in optional
                from y in enumerable
                select x + y;
            return result.Wrap();
        }

        private static readonly IEnumerable<TestCaseData> Cases2 = Cases.Append(Create(None, Empty, SomeEmpty));

        [TestCaseSource(nameof(Cases2))]
        public Optional<IEnumerable<int>> Enumerable_Optional(Optional<int> optional, IEnumerable<int> enumerable)
        {
            var result = from x in enumerable
                from y in optional
                select x + y;
            return result.Wrap();
        }
    }
}