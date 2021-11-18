using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.SelectMany.Optional
{
    [TestFixture]
    internal class Options2Enumerable1_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some4 = Optional<int>.Some(4);
        private static readonly Optional<int> Some17 = Optional<int>.Some(17);
        private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

        private static TestCaseData Create(
            Optional<int> option1,
            Optional<int> option2,
            IEnumerable<int> enumerable,
            IEnumerable<int> result)
        {
            return new(option1, option2, enumerable) { ExpectedResult = result };
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
        public IEnumerable<int> Option_Option_Enumerable(
            Optional<int> option1,
            Optional<int> option2,
            IEnumerable<int> enumerable)
        {
            return
                from x in option1
                from y in option2
                from z in enumerable
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Option_Enumerable_Option(
            Optional<int> option1,
            Optional<int> option2,
            IEnumerable<int> enumerable)
        {
            return
                from x in option1
                from y in enumerable
                from z in option2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Enumerable_Option_Option(
            Optional<int> option1,
            Optional<int> option2,
            IEnumerable<int> enumerable)
        {
            return
                from x in enumerable
                from y in option1
                from z in option2
                select x + y + z;
        }
    }
}