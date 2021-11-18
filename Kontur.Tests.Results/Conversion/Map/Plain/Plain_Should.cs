using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Map.Plain
{
    [TestFixture]
    internal class Plain_Should
    {
        private const int ExpectedFault = 42;

        private static readonly Func<Result<string>, Result<int>>[] Methods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static readonly IEnumerable<TestCaseData> MapCases =
            from testCase in new[]
            {
                (Source: Result<string>.Succeed(), Result: Result<int>.Succeed()),
                (Source: Result<string>.Fail("unused"), Result: Result<int>.Fail(ExpectedFault)),
            }
            from method in Methods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(MapCases))]
        public Result<int> Convert(Result<string> source, Func<Result<string>, Result<int>> converter)
        {
            return converter(source);
        }

        [Test]
        public void Use_Fault()
        {
            var source = Result<int>.Fail(777);

            var result = source.MapFault(i => i.ToString(CultureInfo.InvariantCulture));

            result.Should().BeEquivalentTo(Result<string>.Fail("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Fault factory should not be called on Success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<string>, Result<int>> assertMapped)
        {
            return new(assertMapped);
        }

        private static readonly TestCaseData[] CreateDoNoCallFaultFactoryIfSuccessCases =
        {
            CreateDoNoCallFactoryCase(result => result.MapFault(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.MapFault(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFaultFactoryIfSuccessCases))]
        public void Do_Not_Call_Delegate_If_Success(Func<Result<string>, Result<int>> assertMapped)
        {
            var result = Result<string>.Succeed();

            assertMapped(result);
        }
    }
}