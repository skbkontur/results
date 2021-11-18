using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Map.TValue.Fault
{
    [TestFixture]
    internal class Task_Should
    {
        private const int ExpectedFault = 42;

        private static readonly Func<Task<Result<string, Guid>>, Task<Result<int, Guid>>>[] ResultMethods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static readonly Func<Task<IResult<string, Guid>>, Task<Result<int, Guid>>>[] InterfaceResultMethods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static IEnumerable<TestCaseData> GenerateMapCases<TResult>(
            Func<Guid, TResult> successFactory,
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<Result<int, Guid>>>> methods)
            where TResult : IResult<string, Guid>
        {
            var value = Guid.NewGuid();
            return from testCase in new[]
                {
                    (Source: successFactory(value), Result: Result<int, Guid>.Succeed(value)),
                    (Source: failure, Result: Result<int, Guid>.Fail(ExpectedFault)),
                }
                   from method in methods
                   select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        private static readonly IEnumerable<TestCaseData> ConvertCases =
            GenerateMapCases(Result<string, Guid>.Succeed, Result<string, Guid>.Fail("unused"), ResultMethods)
                .Concat(GenerateMapCases(Result<string, Guid>.Succeed, Result<string, Guid>.Fail("unused"), InterfaceResultMethods));

        [TestCaseSource(nameof(ConvertCases))]
        public Task<Result<int, Guid>> Convert<TResult>(TResult result, Func<Task<TResult>, Task<Result<int, Guid>>> converter)
        {
            var task = Task.FromResult(result);

            return converter(task);
        }

        private static TestCaseData CreateUseFaultCase<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<Result<string, Guid>>> callMap)
            where TResult : IResult<int, Guid>
        {
            return new(factory, callMap);
        }

        private static readonly TestCaseData[] UseFaultCases =
        {
            CreateUseFaultCase(Result<int, Guid>.Fail, (result, map) => result.MapFault(map)),
            CreateUseFaultCase<IResult<int, Guid>>(Result<int, Guid>.Fail, (result, map) => result.MapFault(map)),
        };

        [TestCaseSource(nameof(UseFaultCases))]
        public async Task Use_Fault<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<Result<string, Guid>>> callMap)
            where TResult : IResult<int, Guid>
        {
            var source = Task.FromResult(factory(777));

            var result = await callMap(source, i => i.ToString(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);

            result.Should().BeEquivalentTo(Result<string, Guid>.Fail("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Fault factory should not be called on Success");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<string, Guid>>, Task<Result<int, Guid>>>[] ResultAssertNotCalledMethods =
        {
            result => result.MapFault(_ => AssertIsNotCalled()),
            result => result.MapFault(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<string, Guid>>, Task<Result<int, Guid>>>[] InterfaceResultAssertMotCalledMethods =
        {
            result => result.MapFault(_ => AssertIsNotCalled()),
            result => result.MapFault(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNoCallFaultFactoryIfSuccessCases<TResult>(
            TResult succeed,
            IEnumerable<Func<Task<TResult>, Task<Result<int, Guid>>>> methods)
            where TResult : IResult<string, Guid>
        {
            return methods.Select(method => new TestCaseData(succeed, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNoCallFaultFactoryIfSuccessCases =
            CreateDoNoCallFaultFactoryIfSuccessCases(Result<string, Guid>.Succeed(Guid.NewGuid()), ResultAssertNotCalledMethods)
                .Concat(CreateDoNoCallFaultFactoryIfSuccessCases(Result<string, Guid>.Succeed(Guid.NewGuid()), InterfaceResultAssertMotCalledMethods));

        [TestCaseSource(nameof(DoNoCallFaultFactoryIfSuccessCases))]
        public async Task Do_Not_Call_Delegate_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<Result<int, Guid>>> assertMapped)
            where TResult : IResult<string, Guid>
        {
            var task = Task.FromResult(result);

            await assertMapped(task).ConfigureAwait(false);
        }
    }
}