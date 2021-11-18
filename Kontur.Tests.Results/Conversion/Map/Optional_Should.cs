using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Map
{
    [TestFixture]
    internal class Optional_Should
    {
        private const int ExpectedResult = 42;

        private static readonly Func<Optional<string>, Optional<int>>[] Methods =
        {
            result => result.MapValue(_ => ExpectedResult),
            result => result.MapValue(() => ExpectedResult),
            result => result.MapValue(ExpectedResult),
        };

        private static readonly IEnumerable<TestCaseData> MapCases =
            from testCase in new[]
            {
                (Source: Optional<string>.None(), Result: Optional<int>.None()),
                (Source: Optional<string>.Some("unused"), Result: Optional<int>.Some(ExpectedResult)),
            }
            from method in Methods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(MapCases))]
        public Optional<int> Convert(Optional<string> source, Func<Optional<string>, Optional<int>> converter)
        {
            return converter(source);
        }

        [Test]
        public void Use_Value()
        {
            var option = Optional<int>.Some(777);

            var result = option.MapValue(i => i.ToString(CultureInfo.InvariantCulture));

            result.Should().BeEquivalentTo(Optional<string>.Some("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Value factory should not be called on None");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Optional<string>, Optional<int>> assertMapped)
        {
            return new(assertMapped);
        }

        private static readonly TestCaseData[] CreateDoNoCallSomeFactoryIfNoneCases =
        {
            CreateDoNoCallFactoryCase(option => option.MapValue(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(option => option.MapValue(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallSomeFactoryIfNoneCases))]
        public void Do_Not_Call_Delegate_If_None(Func<Optional<string>, Optional<int>> assertMapped)
        {
            var option = Optional<string>.None();

            assertMapped(option);
        }
    }
}