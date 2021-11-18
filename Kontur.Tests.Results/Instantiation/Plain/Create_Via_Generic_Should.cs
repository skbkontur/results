using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain
{
    [TestFixture(10)]
    [TestFixture("bar")]
    internal class Create_Via_Generic_Should<T>
    {
        private readonly T example;

        public Create_Via_Generic_Should(T example)
        {
            this.example = example;
        }

        private static TestCaseData CreateCase(Func<T, Result<T>> resultFactory, bool success)
        {
            return new(resultFactory) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] CreateCases =
        {
            CreateCase(_ => Result<T>.Succeed(), true),
            CreateCase(Result<T>.Fail, false),
        };

        [TestCaseSource(nameof(CreateCases))]
        public bool Success(Func<T, Result<T>> resultFactory)
        {
            var result = resultFactory(example);

            return result.Success;
        }

        [Test]
        public void Store_Fault()
        {
            var result = Result<T>.Fail(example);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(example);
        }
    }
}
