using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Map.TValue.Fault
{
    [TestFixture]
    internal class Plain_Should
    {
        private const int ExpectedFault = 42;

        private static readonly Func<Result<string, Guid>, Result<int, Guid>>[] Methods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static IEnumerable<TestCaseData> MapCases()
        {
            var value = Guid.NewGuid();
            return from testCase in new[]
                {
                    (Source: Result<string, Guid>.Succeed(value), Result: Result<int, Guid>.Succeed(value)),
                    (Source: Result<string, Guid>.Fail("unused"), Result: Result<int, Guid>.Fail(ExpectedFault)),
                }
                from method in Methods
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(MapCases))]
        public Result<int, Guid> Convert(Result<string, Guid> source, Func<Result<string, Guid>, Result<int, Guid>> converter)
        {
            return converter(source);
        }

        [Test]
        public void Use_Fault()
        {
            var source = Result<int, Guid>.Fail(777);

            var result = source.MapFault(i => i.ToString(CultureInfo.InvariantCulture));

            result.Should().BeEquivalentTo(Result<string, Guid>.Fail("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Fault factory should not be called on Success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<string, Guid>, Result<int, Guid>> assertMapped)
        {
            return new(assertMapped);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryCases =
        {
            CreateDoNoCallFactoryCase(result => result.MapFault(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.MapFault(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryCases))]
        public void Do_Not_Call_Fault_Factory_If_Success(Func<Result<string, Guid>, Result<int, Guid>> assertMapped)
        {
            var result = Result<string, Guid>.Succeed(Guid.Empty);

            assertMapped(result);
        }
    }
}