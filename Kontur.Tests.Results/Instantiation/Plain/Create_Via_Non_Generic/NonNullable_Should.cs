using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain.Create_Via_Non_Generic
{
    [TestFixture(10)]
    [TestFixture("bar")]
    internal class NonNullable_Should<T>
    {
        private readonly T example;

        public NonNullable_Should(T example)
        {
            this.example = example;
        }

        private static TestCaseData Create(Func<T, Result<T>> resultFactory, bool success)
        {
            return new(resultFactory) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(_ => Result.Succeed<T>(), true),
            Create(Result.Fail, false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Success(Func<T, Result<T>> resultFactory)
        {
            var result = resultFactory(example);

            return result.Success;
        }

        [Test]
        public void Store_Fault()
        {
            var result = Result.Fail(example);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(example);
        }
    }
}
