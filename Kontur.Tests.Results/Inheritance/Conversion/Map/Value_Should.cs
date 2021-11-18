using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Conversion.Map
{
    [TestFixture]
    internal class Value_Should
    {
        private const int ExpectedValue = 42;

        private static TestCaseData CreateMapCase(Func<StringFaultResult<Guid>, Result<StringFault, int>> converter)
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
        public void Do_Not_Convert_Failure(Func<StringFaultResult<Guid>, Result<StringFault, int>> converter)
        {
            StringFault expectedFailure = new("bar");

            var source = StringFaultResult.Fail<Guid>(expectedFailure);

            var result = converter(source);

            result.Should().BeEquivalentTo(Result<StringFault, int>.Fail(expectedFailure));
        }

        private static readonly IEnumerable<TestCaseData> ConvertSuccessCases = MapCases
            .Select(testCase => testCase.Returns(Result<StringFault, int>.Succeed(ExpectedValue)));

        [TestCaseSource(nameof(ConvertSuccessCases))]
        public Result<StringFault, int> Convert_Value(Func<StringFaultResult<Guid>, Result<StringFault, int>> converter)
        {
            var result = StringFaultResult.Succeed(Guid.NewGuid());

            return converter(result);
        }

        [Test]
        public void Use_Value()
        {
            var source = StringFaultResult.Succeed(777);

            var result = source.MapValue(i => i.ToString(CultureInfo.InvariantCulture));

            result.Should().BeEquivalentTo(Result<StringFault, string>.Succeed("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Value factory should not be called on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<StringFaultResult<Guid>, Result<StringFault, int>> assertMapped)
        {
            return new(assertMapped);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryCases =
        {
            CreateDoNoCallFactoryCase(result => result.MapValue(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.MapValue(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryCases))]
        public void Do_Not_Call_Success_Factory_If_Failure(Func<StringFaultResult<Guid>, Result<StringFault, int>> assertMapped)
        {
            var result = StringFaultResult.Fail<Guid>(new("unused"));

            assertMapped(result);
        }
    }
}