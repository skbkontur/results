using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Conversion.Map.Fault
{
    [TestFixture]
    internal class Task_Should
    {
        private const int ExpectedFault = 42;

        private static TestCaseData CreateMapCase(Func<Task<StringFaultResult<Guid>>, Task<Result<int, Guid>>> converter)
        {
            return new(converter);
        }

        private static IEnumerable<TestCaseData> MapCases
        {
            get
            {
                yield return CreateMapCase(result => result.MapFault(_ => ExpectedFault));
                yield return CreateMapCase(result => result.MapFault(() => ExpectedFault));
                yield return CreateMapCase(result => result.MapFault(ExpectedFault));
            }
        }

        [TestCaseSource(nameof(MapCases))]
        public async Task Do_Not_Convert_Success(Func<Task<StringFaultResult<Guid>>, Task<Result<int, Guid>>> converter)
        {
            var expectedValue = Guid.NewGuid();

            var source = Task.FromResult(StringFaultResult.Succeed(expectedValue));

            var result = await converter(source).ConfigureAwait(false);

            result.Should().BeEquivalentTo(Result<int, Guid>.Succeed(expectedValue));
        }

        private static readonly IEnumerable<TestCaseData> ConvertFaultCases = MapCases
            .Select(testCase => testCase.Returns(Result<int, Guid>.Fail(ExpectedFault)));

        [TestCaseSource(nameof(ConvertFaultCases))]
        public Task<Result<int, Guid>> Convert_Failure(Func<Task<StringFaultResult<Guid>>, Task<Result<int, Guid>>> converter)
        {
            var result = StringFaultResult.Fail<Guid>(new("unused"));

            return converter(Task.FromResult(result));
        }

        [Test]
        public async Task Use_Fault()
        {
            var source = Task.FromResult(StringFaultResult.Fail<Guid>(new("foo")));

            var result = await source
                .MapFault(i => i + "-bar")
                .ConfigureAwait(false);

            result.Should().BeEquivalentTo(Result<string, Guid>.Fail("foo-bar"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Fault factory should not be called on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Task<StringFaultResult<Guid>>, Task<Result<int, Guid>>> assertMapped)
        {
            return new(assertMapped);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryCases =
        {
            CreateDoNoCallFactoryCase(result => result.MapFault(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.MapFault(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryCases))]
        public async Task Do_Not_Call_Fault_Factory_If_Success(Func<Task<StringFaultResult<Guid>>, Task<Result<int, Guid>>> assertMapped)
        {
            var result = Task.FromResult(StringFaultResult.Succeed(Guid.Empty));

            await assertMapped(result).ConfigureAwait(false);
        }
    }
}