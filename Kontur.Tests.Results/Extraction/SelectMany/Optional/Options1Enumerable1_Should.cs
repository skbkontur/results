using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.Optional
{
    [TestFixture]
    internal class Options1Enumerable1_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Optional<int> optional,
            IEnumerable<int> enumerable,
            IEnumerable<int> result)
        {
            return new(optional, enumerable) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(None, Empty, Empty),
            Create(None, new[] { 3 }, Empty),
            Create(None, new[] { 3, 11 }, Empty),
            Create(Some4, Empty, Empty),
            Create(Some4, new[] { 3 }, new[] { 7 }),
            Create(Some4, new[] { 3, 11 }, new[] { 7, 15 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Option_Enumerable(Optional<int> optional, IEnumerable<int> enumerable)
        {
            return
                from x in optional
                from y in enumerable
                select x + y;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Option(Optional<int> optional, IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in optional
                select x + y;
        }
    }
}