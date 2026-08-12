using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnFailure.TValue
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

        private static readonly Func<Result<string, Guid>, ICounter, ValueTask<Result<string, Guid>>>[] CallMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).OnFailure(counter.Increment),
            (result, counter) => result.OnFailure(_ => IncrementValueTask(counter)),
        };

        private static readonly Func<Result<string, Guid>, ICounter, Task<Result<string, Guid>>>[] CallMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).OnFailure(counter.Increment),
            (result, counter) => Task.FromResult(result).OnFailure(_ => counter.Increment()),
            (result, counter) => Task.FromResult(result).OnFailure(_ => IncrementTask(counter)),
            (result, counter) => ValueTaskFactory.Create(result).OnFailure(_ => IncrementTask(counter)),
        };

        private static IEnumerable<Func<Result<string, Guid>, ICounter, Task<Result<string, Guid>>>> CallMethods()
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

        private static readonly IEnumerable<TestCaseData> CallOnFailureIfFailureCases =
            CallMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public async Task Call_OnFailure_If_Failure(
            Func<Result<string, Guid>, ICounter, Task<Result<string, Guid>>> callOnFailure)
        {
            var counter = Substitute.For<ICounter>();

            await callOnFailure(Result<string, Guid>.Fail("err"), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        [Test]
        public async Task Pass_Fault_If_Failure()
        {
            const string expected = "foo";
            var consumer = Substitute.For<IConsumer>();

            await Task.FromResult(Result<string, Guid>.Fail(expected))
                .OnFailure(value => consumer.Consume(value))
                .ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static void EnsureOnFailureIsNotCalled()
        {
            Assert.Fail("OnFailure is called");
        }

        private static readonly Func<Result<string, Guid>, Task<Result<string, Guid>>>[] ResultNotCalledMethods =
        {
            result => Task.FromResult(result).OnFailure(_ => EnsureOnFailureIsNotCalled()),
            result => Task.FromResult(result).OnFailure(EnsureOnFailureIsNotCalled),
        };

        private static readonly Func<IResult<string, Guid>, Task<Result<string, Guid>>>[] InterfaceNotCalledMethods =
        {
            result => Task.FromResult(result).OnFailure(_ => EnsureOnFailureIsNotCalled()),
            result => Task.FromResult(result).OnFailure(EnsureOnFailureIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TResult>(
            TResult success,
            IEnumerable<Func<TResult, Task<Result<string, Guid>>>> methods)
            where TResult : IResult<string, Guid>
        {
            return methods.Select(method => new TestCaseData(success, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnFailureIfSuccessCases =
            CreateDoNotCallCases(Result<string, Guid>.Succeed(Guid.NewGuid()), ResultNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<string, Guid>>(Result<string, Guid>.Succeed(Guid.NewGuid()), InterfaceNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnFailureIfSuccessCases))]
        public async Task Do_Not_Call_OnFailure_If_Success<TResult>(
            TResult result,
            Func<TResult, Task<Result<string, Guid>>> assertNotCalled)
            where TResult : IResult<string, Guid>
        {
            await assertNotCalled(result).ConfigureAwait(false);
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<TResult, Task<Result<int, Guid>>> method)
            where TResult : IResult<int, Guid>
        {
            return new(source, method) { ExpectedResult = source };
        }

        private static readonly Func<Result<int, Guid>, Task<Result<int, Guid>>>[] ResultReturnMethods =
        {
            result => Task.FromResult(result).OnFailure(_ => { }),
            result => Task.FromResult(result).OnFailure(() => { }),
        };

        private static readonly Func<IResult<int, Guid>, Task<Result<int, Guid>>>[] InterfaceReturnMethods =
        {
            result => Task.FromResult(result).OnFailure(_ => { }),
            result => Task.FromResult(result).OnFailure(() => { }),
        };

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<int, Guid>>>> methods)
            where TResult : IResult<int, Guid>
        {
            return methods.Select(method => CreateReturnCase(source, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            GenerateReturnCases(Result<int, Guid>.Fail(123), ResultReturnMethods)
                .Concat(GenerateReturnCases<IResult<int, Guid>>(Result<int, Guid>.Fail(123), InterfaceReturnMethods))
                .Concat(GenerateReturnCases(Result<int, Guid>.Succeed(Guid.NewGuid()), ResultReturnMethods))
                .Concat(GenerateReturnCases<IResult<int, Guid>>(Result<int, Guid>.Succeed(Guid.NewGuid()), InterfaceReturnMethods));

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Task<Result<int, Guid>> Return_Self<TResult>(
            TResult result,
            Func<TResult, Task<Result<int, Guid>>> callOnFailure)
            where TResult : IResult<int, Guid>
        {
            return callOnFailure(result);
        }
    }
}
