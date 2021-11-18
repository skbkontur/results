using System.Collections.Generic;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators
{
    [TestFixture]
    internal class SelectCaseGenerator_Should
    {
        private static readonly Result<string, int> Failure10 = CreateFailure("foo-10");
        private static readonly Result<string, int> Failure11 = CreateFailure("foo-11");
        private static readonly Result<string, int> Failure12 = CreateFailure("foo-12");
        private static readonly Result<string, int> Success10 = CreateSuccess(10);
        private static readonly Result<string, int> Success11 = CreateSuccess(11);
        private static readonly Result<string, int> Success12 = CreateSuccess(12);

        private static Result<string, int> CreateSuccess(int value) => Result<string, int>.Succeed(value);

        private static Result<string, int> CreateFailure(string fault) => Result<string, int>.Fail(fault);

        private static TestCaseData Create(int argsCount, params SelectCase[] expectedResult)
        {
            return new(argsCount, expectedResult);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(
                1,
                new(new[] { Failure10 }, Failure10),
                new(new[] { Success10 }, Success10)),
            Create(
                2,
                new(new[] { Failure10, Failure11 }, Failure10),
                new(new[] { Success10, Failure11 }, Failure11),
                new(new[] { Failure10, Success11 }, Failure10),
                new(new[] { Success10, Success11 }, CreateSuccess(21))),
            Create(
                3,
                new(new[] { Failure10, Failure11, Failure12 }, Failure10),
                new(new[] { Success10, Failure11, Failure12 }, Failure11),
                new(new[] { Failure10, Success11, Failure12 }, Failure10),
                new(new[] { Success10, Success11, Failure12 }, Failure12),
                new(new[] { Failure10, Failure11, Success12 }, Failure10),
                new(new[] { Success10, Failure11, Success12 }, Failure11),
                new(new[] { Failure10, Success11, Success12 }, Failure10),
                new(new[] { Success10, Success11, Success12 }, CreateSuccess(33))),
        };

        [TestCaseSource(nameof(Cases))]
        public void Construct_Cases(int argsCount, IEnumerable<SelectCase> expectedResult)
        {
            var cases = SelectCasesGenerator.Create(argsCount);

            cases.Should().BeEquivalentTo(expectedResult);
        }
    }
}
