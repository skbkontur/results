using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Optional
{
    [TestFixture(10)]
    [TestFixture("bar")]
    internal class Create_Via_Generic_Should<T>
    {
        private readonly T value;

        public Create_Via_Generic_Should(T value)
        {
            this.value = value;
        }

        private static TestCaseData CreateCase(Func<T, Optional<T>> optionalFactory, bool hasSome)
        {
            return new(optionalFactory) { ExpectedResult = hasSome };
        }

        private static readonly TestCaseData[] CreateCases =
        {
            CreateCase(_ => Optional<T>.None(), false),
            CreateCase(Optional<T>.Some, true),
        };

        [TestCaseSource(nameof(CreateCases))]
        public bool HasValue(Func<T, Optional<T>> optionalFactory)
        {
            var optional = optionalFactory(this.value);

            return optional.HasSome;
        }

        [Test]
        public void Store_Value()
        {
            var optional = Optional<T>.Some(this.value);

            var result = optional.GetValueOrThrow();

            result.Should().Be(this.value);
        }
    }
}
