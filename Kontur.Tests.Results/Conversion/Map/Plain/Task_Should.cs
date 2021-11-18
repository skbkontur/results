using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Map.Plain
{
    [TestFixture]
    internal class Task_Should
    {
        private const int ExpectedFault = 42;

        private static TestCaseData CreateConvertCase<TResult, TExpectedResult>(
            TResult source,
            Result<int> result,
            Func<Task<TResult>, Task<TExpectedResult>> method)
            where TResult : IResult<string>
            where TExpectedResult : IResult<int>
        {
            return new(source, method)
            {
                ExpectedResult = result,
            };
        }

        private static readonly Func<Task<Result<string>>, Task<Result<int>>>[] ResultMethods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static readonly Func<Task<IResult<string>>, Task<Result<int>>>[] InterfaceResultMethods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static readonly Func<Task<ResultFailure<string>>, Task<ResultFailure<int>>>[] ResultFailureMethods =
        {
            result => result.MapFault(_ => ExpectedFault),
            result => result.MapFault(() => ExpectedFault),
            result => result.MapFault(ExpectedFault),
        };

        private static IEnumerable<TestCaseData> GenerateMapCases<TResult, TExpectedResult>(
            TResult success,
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<TExpectedResult>>> methods)
            where TResult : IResult<string>
            where TExpectedResult : IResult<int>
        {
            return from testCase in new[]
                {
                    (Source: success, Result: Result<int>.Succeed()),
                    (Source: failure, Result: Result<int>.Fail(ExpectedFault)),
                }
                from method in methods
                select CreateConvertCase(testCase.Source, testCase.Result, method);
        }

        private static readonly IEnumerable<TestCaseData> ConvertCases =
            GenerateMapCases(Result<string>.Succeed(), Result<string>.Fail("unused"), ResultMethods)
                .Concat(GenerateMapCases(Result<string>.Succeed(), Result<string>.Fail("unused"), InterfaceResultMethods))
                .Concat(ResultFailureMethods.Select(method =>
                    CreateConvertCase(Result.Fail("unused"), Result<int>.Fail(ExpectedFault), method)));

        [TestCaseSource(nameof(ConvertCases))]
        public Task<TExpectedResult> Convert<TResult, TExpectedResult>(TResult result, Func<Task<TResult>, Task<TExpectedResult>> converter)
            where TResult : IResult<string>
            where TExpectedResult : IResult<int>
        {
            var task = Task.FromResult(result);

            return converter(task);
        }

        private static TestCaseData CreateUseFaultCase<TResult, TExpectedResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<TExpectedResult>> callMap)
            where TResult : IResult<int>
            where TExpectedResult : IResult<string>
        {
            return new(factory, callMap);
        }

        private static readonly TestCaseData[] UseFaultCases =
        {
            CreateUseFaultCase(Result<int>.Fail, (result, map) => result.MapFault(map)),
            CreateUseFaultCase<IResult<int>, Result<string>>(Result<int>.Fail, (result, map) => result.MapFault(map)),
            CreateUseFaultCase(Result.Fail, (result, map) => result.MapFault(map)),
        };

        [TestCaseSource(nameof(UseFaultCases))]
        public async Task Use_Fault<TResult, TExpectedResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<TExpectedResult>> callMap)
            where TResult : IResult<int>
            where TExpectedResult : IResult<string>
        {
            var source = Task.FromResult(factory(777));

            var result = await callMap(source, i => i.ToString(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);

            result.Should().BeEquivalentTo(Result<string>.Fail("777"));
        }

        private static int AssertIsNotCalled()
        {
            Assert.Fail("Fault factory should not be called on Success");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<string>>, Task<Result<int>>>[] ResultAssertNotCalledMethods =
        {
            result => result.MapFault(_ => AssertIsNotCalled()),
            result => result.MapFault(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<string>>, Task<Result<int>>>[] InterfaceResultAssertMotCalledMethods =
        {
            result => result.MapFault(_ => AssertIsNotCalled()),
            result => result.MapFault(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNoCallFaultFactoryIfSuccessCases<TResult>(
            TResult succeed,
            IEnumerable<Func<Task<TResult>, Task<Result<int>>>> methods)
            where TResult : IResult<string>
        {
            return methods.Select(method => new TestCaseData(succeed, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNoCallFaultFactoryIfSuccessCases =
            CreateDoNoCallFaultFactoryIfSuccessCases(Result<string>.Succeed(), ResultAssertNotCalledMethods)
                .Concat(CreateDoNoCallFaultFactoryIfSuccessCases(Result<string>.Succeed(), InterfaceResultAssertMotCalledMethods));

        [TestCaseSource(nameof(DoNoCallFaultFactoryIfSuccessCases))]
        public async Task Do_Not_Call_Delegate_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<Result<int>>> assertMapped)
            where TResult : IResult<string>
        {
            var task = Task.FromResult(result);

            await assertMapped(task).ConfigureAwait(false);
        }
    }
}