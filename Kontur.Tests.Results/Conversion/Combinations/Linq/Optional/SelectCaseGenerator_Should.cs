using System.Collections.Generic;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional
{
    [TestFixture]
    internal class SelectCaseGenerator_Should
    {
        private static readonly Optional<int> None = Optional<int>.None();
        private static readonly Optional<int> Some10 = CreateSome(10);
        private static readonly Optional<int> Some11 = CreateSome(11);
        private static readonly Optional<int> Some12 = CreateSome(12);

        private static Optional<int> CreateSome(int value) => Optional<int>.Some(value);

        private static TestCaseData Create(int argsCount, params SelectCase[] expectedResult)
        {
            return new(argsCount, expectedResult);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(
                1,
                new(new[] { None }, None),
                new(new[] { Some10 }, Some10)),
            Create(
                2,
                new(new[] { None, None }, None),
                new(new[] { Some10, None }, None),
                new(new[] { None, Some11 }, None),
                new(new[] { Some10, Some11 }, CreateSome(21))),
            Create(
                3,
                new(new[] { None, None, None }, None),
                new(new[] { Some10, None, None }, None),
                new(new[] { None, Some11, None }, None),
                new(new[] { Some10, Some11, None }, None),
                new(new[] { None, None, Some12 }, None),
                new(new[] { Some10, None, Some12 }, None),
                new(new[] { None, Some11, Some12 }, None),
                new(new[] { Some10, Some11, Some12 }, CreateSome(33))),
        };

        [TestCaseSource(nameof(Cases))]
        public void Construct_Cases(int argsCount, IEnumerable<SelectCase> expectedResult)
        {
            var cases = SelectCasesGenerator.Create(argsCount);

            cases.Should().BeEquivalentTo(expectedResult);
        }
    }
}
