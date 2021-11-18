using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue
{
    [TestFixture(10)]
    [TestFixture("bar")]
    internal class Create_Via_Equals_Generic_Should<T>
    {
        private readonly T example;

        public Create_Via_Equals_Generic_Should(T example)
        {
            this.example = example;
        }

        private static TestCaseData CreateCase(Func<T, Result<T, T>> resultFactory, bool success)
        {
            return new(resultFactory) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] CreateCases =
        {
            CreateCase(Result<T, T>.Fail, false),
            CreateCase(Result<T, T>.Succeed, true),
        };

        [TestCaseSource(nameof(CreateCases))]
        public bool HasValue(Func<T, Result<T, T>> resultFactory)
        {
            var result = resultFactory(example);

            return result.Success;
        }

        [Test]
        public void Store_Value()
        {
            var result = Result<T, T>.Succeed(example);

            var value = result.GetValueOrThrow();

            value.Should().Be(example);
        }

        [Test]
        public void Store_Fault()
        {
            var result = Result<T, T>.Fail(example);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(example);
        }
    }
}
