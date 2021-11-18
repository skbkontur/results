using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq
{
    [TestFixture]
    internal class KPermutationOfBool_Should
    {
        private static TestCaseData Create(int length, params IEnumerable<bool>[] expectedResult)
        {
            return new(length, expectedResult);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(0),
            Create(1, new[] { false }, new[] { true }),
            Create(
                2,
                new[] { false, false },
                new[] { false, true },
                new[] { true, false },
                new[] { true, true }),
            Create(
                3,
                new[] { false, false, false },
                new[] { false, false, true },
                new[] { false, true, false },
                new[] { false, true, true },
                new[] { true, false, false },
                new[] { true, false, true },
                new[] { true, true, false },
                new[] { true, true, true }),
        };

        [TestCaseSource(nameof(Cases))]
        public void Create_Permutations(int length, IEnumerable<IEnumerable<bool>> expectedResult)
        {
            var result = KPermutationOfBool.Create(length);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
