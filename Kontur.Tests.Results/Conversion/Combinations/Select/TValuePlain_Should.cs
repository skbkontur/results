using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Select
{
    [TestFixture]
    internal class TValuePLain_Should
    {
        private static TestCaseData Create(Result<int, string> source, Result<int> result)
        {
            return new(source) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<int, string>.Fail(10), Result<int>.Fail(10)),
            Create(Result<int, string>.Succeed("unused"), Result<int>.Succeed()),
        };

        [TestCaseSource(nameof(Cases))]
        public Result<int> Process(Result<int, string> result)
        {
            return result.Select(_ => Result<int>.Succeed());
        }

        [Test]
        public void Convert_Success_To_Failure()
        {
            const int expected = 20;
            var source = Result<string, int>.Succeed(expected);

            var result = source.Select(ResultFactory.CreateFailure);

            result.Should().BeEquivalentTo(ResultFactory.CreateFailure(expected));
        }
    }
}
