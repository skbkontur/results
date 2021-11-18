using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue.Create_Via_Non_Generic
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

        private static TestCaseData Create(Func<T, Result<T, T>> resultFactory, bool success)
        {
            return new(resultFactory) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(ResultFailure<T>.Create, false),
            Create(Result<T>.Succeed, true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool HasValue(Func<T, Result<T, T>> resultFactory)
        {
            var result = resultFactory(example);

            return result.Success;
        }

        [Test]
        public void Store_Value()
        {
            var result = Result<Guid>.Succeed(example);

            var value = result.GetValueOrThrow();

            value.Should().Be(example);
        }

        [Test]
        public void Store_Value_For_Same_Types()
        {
            var result = Result<T>.Succeed(example);

            var value = result.GetValueOrThrow();

            value.Should().Be(example);
        }

        [Test]
        public void Store_Fault()
        {
            var result = ResultFailure<Guid>.Create(example);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(example);
        }

        [Test]
        public void Store_Fault_For_Same_Types()
        {
            var result = ResultFailure<T>.Create(example);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(example);
        }
    }
}
