using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Map.TValue
{
    [TestFixture]
    internal class Value_Should
    {
        private const int ExpectedValue = 42;

        private static TestCaseData CreateMapCase(Func<Result<Guid, string>, Result<Guid, int>> converter)
        {
            return new(converter);
        }

        private static IEnumerable<TestCaseData> MapCases
        {
            get
            {
                yield return CreateMapCase(result => result.MapValue(_ => ExpectedValue));
                yield return CreateMapCase(result => result.MapValue(() => ExpectedValue));
                yield return CreateMapCase(result => result.MapValue(ExpectedValue));
            }
        }

        [TestCaseSource(nameof(MapCases))]
        public void Do_Not_Convert_Failure(Func<Result<Guid, string>, Result<Guid, int>> converter)
        {
            var expectedFailure = Guid.NewGuid();

            var source = Result<Guid, string>.Fail(expectedFailure);

            var result = converter(source);

            result.Should().BeEquivalentTo(Result<Guid, int>.Fail(expectedFailure));
        }

        private static readonly IEnumerable<TestCaseData> ConvertSuccessCases = MapCases
            .Select(testCase => testCase.Returns(Result<Guid, int>.Succeed(ExpectedValue)));

        [TestCaseSource(nameof(ConvertSuccessCases))]
        public Result<Guid, int> Convert_Value(Func<Result<Guid, string>, Result<Guid, int>> converter)
        {
            var result = Result<Guid, string>.Succeed("unused");

            return converter(result);
        }

        [Test]
        public void Use_Value()
        {
            var source = Result<Guid, int>.Succeed(777);

            var result = source.MapValue(i => i.ToString(CultureInfo.InvariantCulture));

            result.Should().BeEquivalentTo(Result<Guid, string>.Succeed("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Value factory should not be called on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<Guid, string>, Result<Guid, int>> assertMapped)
        {
            return new(assertMapped);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryCases =
        {
            CreateDoNoCallFactoryCase(result => result.MapValue(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.MapValue(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryCases))]
        public void Do_Not_Call_Success_Factory_If_Failure(Func<Result<Guid, string>, Result<Guid, int>> assertMapped)
        {
            var result = Result<Guid, string>.Fail(Guid.Empty);

            assertMapped(result);
        }
    }
}