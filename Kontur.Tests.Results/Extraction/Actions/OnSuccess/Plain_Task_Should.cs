using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnSuccess
{
    [TestFixture]
    internal class Plain_Task_Should
    {
        private static Task IncrementTask(ICounter counter)
        {
            counter.Increment();

            return Task.CompletedTask;
        }

        private static ValueTask IncrementValueTask(ICounter counter)
        {
            counter.Increment();

            return default;
        }

        private static readonly Func<Result<string>, ICounter, ValueTask<Result<string>>>[] CallMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).OnSuccess(counter.Increment),
            (result, counter) => result.OnSuccess(() => IncrementValueTask(counter)),
        };

        private static readonly Func<Result<string>, ICounter, Task<Result<string>>>[] CallMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).OnSuccess(counter.Increment),
            (result, counter) => Task.FromResult(result).OnSuccess(() => IncrementTask(counter)),
            (result, counter) => ValueTaskFactory.Create(result).OnSuccess(() => IncrementTask(counter)),
        };

        private static IEnumerable<Func<Result<string>, ICounter, Task<Result<string>>>> CallMethods()
        {
            foreach (var method in CallMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in CallMethodsValueTasks)
            {
                yield return async (result, counter) => await method(result, counter).ConfigureAwait(false);
            }
        }

        private static readonly IEnumerable<TestCaseData> CallOnSuccessIfSuccessCases =
            CallMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public async Task Call_OnSuccess_If_Success(
            Func<Result<string>, ICounter, Task<Result<string>>> callOnSuccess)
        {
            var counter = Substitute.For<ICounter>();

            await callOnSuccess(Result<string>.Succeed(), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly Func<Result<string>, Task<Result<string>>>[] ResultNotCalledMethods =
        {
            result => Task.FromResult(result).OnSuccess(EnsureOnSuccessIsNotCalled),
        };

        private static readonly Func<IResult<string>, Task<Result<string>>>[] InterfaceNotCalledMethods =
        {
            result => Task.FromResult(result).OnSuccess(EnsureOnSuccessIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TResult>(
            TResult failure,
            IEnumerable<Func<TResult, Task<Result<string>>>> methods)
            where TResult : IResult<string>
        {
            return methods.Select(method => new TestCaseData(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnSuccessIfFailureCases =
            CreateDoNotCallCases(Result<string>.Fail("err"), ResultNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<string>>(Result<string>.Fail("err"), InterfaceNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public async Task Do_Not_Call_OnSuccess_If_Failure<TResult>(
            TResult result,
            Func<TResult, Task<Result<string>>> assertNotCalled)
            where TResult : IResult<string>
        {
            await assertNotCalled(result).ConfigureAwait(false);
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<TResult, Task<Result<int>>> method)
            where TResult : IResult<int>
        {
            return new(source, method) { ExpectedResult = source };
        }

        private static readonly Func<Result<int>, Task<Result<int>>>[] ResultReturnMethods =
        {
            result => Task.FromResult(result).OnSuccess(() => { }),
        };

        private static readonly Func<IResult<int>, Task<Result<int>>>[] InterfaceReturnMethods =
        {
            result => Task.FromResult(result).OnSuccess(() => { }),
        };

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<int>>>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => CreateReturnCase(source, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            GenerateReturnCases(Result<int>.Succeed(), ResultReturnMethods)
                .Concat(GenerateReturnCases<IResult<int>>(Result<int>.Succeed(), InterfaceReturnMethods))
                .Concat(GenerateReturnCases(Result<int>.Fail(1), ResultReturnMethods))
                .Concat(GenerateReturnCases<IResult<int>>(Result<int>.Fail(1), InterfaceReturnMethods));

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Task<Result<int>> Return_Self_OnSuccess<TResult>(
            TResult result,
            Func<TResult, Task<Result<int>>> callOnSuccess)
            where TResult : IResult<int>
        {
            return callOnSuccess(result);
        }
    }
}
