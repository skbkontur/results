using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue
{
    [TestFixture]
    internal class Create_Via_Not_Equals_Generic_Should
    {
        private static TestCaseData CreateCase(Result<string, int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] CreateCases =
        {
            CreateCase(Result<string, int>.Fail("error"), false),
            CreateCase(Result<string, int>.Succeed(42), true),
        };

        [TestCaseSource(nameof(CreateCases))]
        public bool HasValue(Result<string, int> result)
        {
            return result.Success;
        }

        [Test]
        public void Store_Value()
        {
            var expected = Guid.NewGuid();

            var result = Result<string, Guid>.Succeed(expected);

            var value = result.GetValueOrThrow();

            value.Should().Be(expected);
        }

        [Test]
        public void Store_Fault()
        {
            const string description = "error";

            var result = Result<string, int>.Fail(description);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(description);
        }
    }
}
