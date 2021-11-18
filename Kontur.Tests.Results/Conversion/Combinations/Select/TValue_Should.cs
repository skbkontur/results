using System;
using System.Globalization;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Select
{
    [TestFixture]
    internal class TValue_Should
    {
        private static TestCaseData Create(Result<Guid, int> source, Result<Guid, string> result)
        {
            return new(source) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<Guid, int>.Fail(Guid.Empty), Result<Guid, string>.Fail(Guid.Empty)),
            Create(Result<Guid, int>.Succeed(1), Result<Guid, string>.Succeed("1")),
        };

        [TestCaseSource(nameof(Cases))]
        public Result<Guid, string> Process_Value(Result<Guid, int> result)
        {
            return result.Select(i => i.ToString(CultureInfo.InvariantCulture));
        }

        [TestCaseSource(nameof(Cases))]
        public Result<Guid, string> Process_Result(Result<Guid, int> result)
        {
            return result.Select(i => Result<Guid, string>.Succeed(i.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void Convert_Success_To_Failure()
        {
            var source = Result<int, Guid>.Succeed(Guid.NewGuid());

            var result = source.Select(_ => Result<int, int>.Fail(777));

            result.Should().BeEquivalentTo(Result<int, int>.Fail(777));
        }
    }
}
