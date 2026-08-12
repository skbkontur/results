using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnSuccess.TValue
{
    [TestFixture]
    internal class Task_Should
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

        private static readonly Func<Result<Guid, string>, ICounter, ValueTask<Result<Guid, string>>>[] CallMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).OnSuccess(counter.Increment),
            (result, counter) => result.OnSuccess(_ => IncrementValueTask(counter)),
        };

        private static readonly Func<Result<Guid, string>, ICounter, Task<Result<Guid, string>>>[] CallMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).OnSuccess(counter.Increment),
            (result, counter) => Task.FromResult(result).OnSuccess(_ => counter.Increment()),
            (result, counter) => Task.FromResult(result).OnSuccess(_ => IncrementTask(counter)),
            (result, counter) => ValueTaskFactory.Create(result).OnSuccess(_ => IncrementTask(counter)),
        };

        private static IEnumerable<Func<Result<Guid, string>, ICounter, Task<Result<Guid, string>>>> CallMethods()
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
            Func<Result<Guid, string>, ICounter, Task<Result<Guid, string>>> callOnSuccess)
        {
            var counter = Substitute.For<ICounter>();

            await callOnSuccess(Result<Guid, string>.Succeed("foo"), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        [Test]
        public async Task Pass_Value_If_Success()
        {
            const string expected = "foo";
            var consumer = Substitute.For<IConsumer>();

            await Task.FromResult(Result<int, string>.Succeed(expected))
                .OnSuccess(value => consumer.Consume(value))
                .ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly Func<Result<Guid, string>, Task<Result<Guid, string>>>[] ResultNotCalledMethods =
        {
            result => Task.FromResult(result).OnSuccess(_ => EnsureOnSuccessIsNotCalled()),
            result => Task.FromResult(result).OnSuccess(EnsureOnSuccessIsNotCalled),
        };

        private static readonly Func<IResult<Guid, string>, Task<Result<Guid, string>>>[] InterfaceNotCalledMethods =
        {
            result => Task.FromResult(result).OnSuccess(_ => EnsureOnSuccessIsNotCalled()),
            result => Task.FromResult(result).OnSuccess(EnsureOnSuccessIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TResult>(
            TResult failure,
            IEnumerable<Func<TResult, Task<Result<Guid, string>>>> methods)
            where TResult : IResult<Guid, string>
        {
            return methods.Select(method => new TestCaseData(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnSuccessIfFailureCases =
            CreateDoNotCallCases(Result<Guid, string>.Fail(Guid.NewGuid()), ResultNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<Guid, string>>(Result<Guid, string>.Fail(Guid.NewGuid()), InterfaceNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public async Task Do_Not_Call_OnSuccess_If_Failure<TResult>(
            TResult result,
            Func<TResult, Task<Result<Guid, string>>> assertNotCalled)
            where TResult : IResult<Guid, string>
        {
            await assertNotCalled(result).ConfigureAwait(false);
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<TResult, Task<Result<Guid, int>>> method)
            where TResult : IResult<Guid, int>
        {
            return new(source, method) { ExpectedResult = source };
        }

        private static readonly Func<Result<Guid, int>, Task<Result<Guid, int>>>[] ResultReturnMethods =
        {
            result => Task.FromResult(result).OnSuccess(_ => { }),
            result => Task.FromResult(result).OnSuccess(() => { }),
        };

        private static readonly Func<IResult<Guid, int>, Task<Result<Guid, int>>>[] InterfaceReturnMethods =
        {
            result => Task.FromResult(result).OnSuccess(_ => { }),
            result => Task.FromResult(result).OnSuccess(() => { }),
        };

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<Guid, int>>>> methods)
            where TResult : IResult<Guid, int>
        {
            return methods.Select(method => CreateReturnCase(source, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            GenerateReturnCases(Result<Guid, int>.Succeed(123), ResultReturnMethods)
                .Concat(GenerateReturnCases<IResult<Guid, int>>(Result<Guid, int>.Succeed(123), InterfaceReturnMethods))
                .Concat(GenerateReturnCases(Result<Guid, int>.Fail(Guid.NewGuid()), ResultReturnMethods))
                .Concat(GenerateReturnCases<IResult<Guid, int>>(Result<Guid, int>.Fail(Guid.NewGuid()), InterfaceReturnMethods));

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Task<Result<Guid, int>> Return_Self<TResult>(
            TResult result,
            Func<TResult, Task<Result<Guid, int>>> callOnSuccess)
            where TResult : IResult<Guid, int>
        {
            return callOnSuccess(result);
        }
    }
}
