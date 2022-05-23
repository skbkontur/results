using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.Optional
{
    [TestFixture]
    internal class OptionsEnumerable_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();
        private static readonly Optional<IEnumerable<int>> NoneEnumerable = Optional<IEnumerable<int>>.None();

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable,
            Optional<IEnumerable<int>> result)
        {
            return new(optional, enumerable) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, Empty, NoneEnumerable),
            Create(None, new[] { 3 }, NoneEnumerable),
            Create(None, new[] { 3, 11 }, NoneEnumerable),
            Create(Some4, Empty, Optional<IEnumerable<int>>.Some(Enumerable.Empty<int>())),
            Create(Some4, new[] { 3 }, Optional<IEnumerable<int>>.Some(new[] { 7 })),
            Create(Some4, new[] { 3, 11 }, Optional<IEnumerable<int>>.Some(new[] { 7, 15 })),
        };

        [TestCaseSource(nameof(Cases))]
        public Optional<IEnumerable<int>> Optional_Enumerable(Optional<int> optional, IEnumerable<int> enumerable)
        {
            var result = from x in optional
                from y in enumerable
                select x + y;
            return result.Wrap();
        }
    }
}