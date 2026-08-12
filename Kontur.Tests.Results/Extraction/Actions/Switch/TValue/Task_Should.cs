using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.Switch.TValue
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

        private static IEnumerable<Func<Result<int, string>, ICounter, Task<Result<int, string>>>> CreateCallMethods(
            IEnumerable<Func<Result<int, string>, ICounter, Task<Result<int, string>>>> tasks,
            IEnumerable<Func<Result<int, string>, ICounter, ValueTask<Result<int, string>>>> valueTasks)
        {
            foreach (var method in tasks)
            {
                yield return method;
            }

            foreach (var method in valueTasks)
            {
                yield return async (result, counter) => await method(result, counter).ConfigureAwait(false);
            }
        }

        private static readonly Func<Result<int, string>, ICounter, ValueTask<Result<int, string>>>[] CallOnFailureMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).Switch(counter.Increment, _ => { }),
            (result, counter) => result.Switch(_ => IncrementValueTask(counter), _ => { }),
        };

        private static readonly Func<Result<int, string>, ICounter, Task<Result<int, string>>>[] CallOnFailureMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).Switch(_ => counter.Increment(), _ => { }),
            (result, counter) => Task.FromResult(result).Switch(counter.Increment, () => { }),
            (result, counter) => Task.FromResult(result).Switch(_ => IncrementTask(counter), _ => { }),
            (result, counter) => ValueTaskFactory.Create(result).Switch(_ => IncrementTask(counter), _ => { }),
        };

        private static readonly IEnumerable<TestCaseData> CallOnFailureIfFailureCases =
            CreateCallMethods(CallOnFailureMethodsTasks, CallOnFailureMethodsValueTasks)
                .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public async Task Call_OnFailure_If_Failure(
            Func<Result<int, string>, ICounter, Task<Result<int, string>>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();

            await callSwitch(Result<int, string>.Fail(20), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        private static TestCaseData CreateCallSwitchWithConsumerCase(
            Func<Result<string, string>, IConsumer, Task<Result<string, string>>> callSwitch)
        {
            return new(callSwitch);
        }

        private static readonly TestCaseData[] PassValueIfFailureCases =
        {
            CreateCallSwitchWithConsumerCase((result, consumer) => Task.FromResult(result).Switch(consumer.Consume, () => { })),
            CreateCallSwitchWithConsumerCase((result, consumer) => Task.FromResult(result).Switch(consumer.Consume, _ => { })),
        };

        [TestCaseSource(nameof(PassValueIfFailureCases))]
        public async Task Pass_Fault_If_Failure(
            Func<Result<string, string>, IConsumer, Task<Result<string, string>>> callSwitch)
        {
            const string expected = "bar";
            var consumer = Substitute.For<IConsumer>();

            await callSwitch(Result<string, string>.Fail(expected), consumer).ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static readonly Func<Result<int, string>, ICounter, ValueTask<Result<int, string>>>[] CallOnSuccessMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).Switch(_ => { }, counter.Increment),
            (result, counter) => result.Switch(_ => { }, _ => IncrementValueTask(counter)),
        };

        private static readonly Func<Result<int, string>, ICounter, Task<Result<int, string>>>[] CallOnSuccessMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).Switch(_ => { }, _ => counter.Increment()),
            (result, counter) => Task.FromResult(result).Switch(() => { }, counter.Increment),
            (result, counter) => Task.FromResult(result).Switch(_ => { }, _ => IncrementTask(counter)),
            (result, counter) => ValueTaskFactory.Create(result).Switch(_ => { }, _ => IncrementTask(counter)),
        };

        private static readonly IEnumerable<TestCaseData> CallOnSuccessIfSuccessCases =
            CreateCallMethods(CallOnSuccessMethodsTasks, CallOnSuccessMethodsValueTasks)
                .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public async Task Call_OnSuccess_If_Success(
            Func<Result<int, string>, ICounter, Task<Result<int, string>>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();

            await callSwitch(Result<int, string>.Succeed("bar"), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        private static readonly TestCaseData[] PassValueIfSuccessCases =
        {
            CreateCallSwitchWithConsumerCase((result, consumer) => Task.FromResult(result).Switch(() => { }, consumer.Consume)),
            CreateCallSwitchWithConsumerCase((result, consumer) => Task.FromResult(result).Switch(_ => { }, consumer.Consume)),
        };

        [TestCaseSource(nameof(PassValueIfSuccessCases))]
        public async Task Pass_Value_If_Success(
            Func<Result<string, string>, IConsumer, Task<Result<string, string>>> callSwitch)
        {
            const string expected = "foo";
            var consumer = Substitute.For<IConsumer>();

            await callSwitch(Result<string, string>.Succeed(expected), consumer).ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly Func<Result<Guid, string>, Task<Result<Guid, string>>>[] ResultOnSuccessNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(() => { }, EnsureOnSuccessIsNotCalled),
            result => Task.FromResult(result).Switch(() => { }, _ => EnsureOnSuccessIsNotCalled()),
            result => Task.FromResult(result).Switch(_ => { }, EnsureOnSuccessIsNotCalled),
            result => Task.FromResult(result).Switch(_ => { }, _ => EnsureOnSuccessIsNotCalled()),
        };

        private static readonly Func<IResult<Guid, string>, Task<Result<Guid, string>>>[] InterfaceOnSuccessNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(() => { }, EnsureOnSuccessIsNotCalled),
            result => Task.FromResult(result).Switch(() => { }, _ => EnsureOnSuccessIsNotCalled()),
            result => Task.FromResult(result).Switch(_ => { }, EnsureOnSuccessIsNotCalled),
            result => Task.FromResult(result).Switch(_ => { }, _ => EnsureOnSuccessIsNotCalled()),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TResult, TFault, TValue>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<TFault, TValue>>>> methods)
            where TResult : IResult<TFault, TValue>
        {
            return methods.Select(method => new TestCaseData(source, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnSuccessIfFailureCases =
            CreateDoNotCallCases(Result<Guid, string>.Fail(Guid.NewGuid()), ResultOnSuccessNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<Guid, string>, Guid, string>(
                    Result<Guid, string>.Fail(Guid.NewGuid()),
                    InterfaceOnSuccessNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public async Task Do_Not_Call_OnSuccess_If_Failure<TResult>(
            TResult result,
            Func<TResult, Task<Result<Guid, string>>> assertOnSuccessIsNotCalled)
            where TResult : IResult<Guid, string>
        {
            await assertOnSuccessIsNotCalled(result).ConfigureAwait(false);
        }

        private static void EnsureOnFailureIsNotCalled()
        {
            Assert.Fail("OnFailure is called");
        }

        private static readonly Func<Result<string, int>, Task<Result<string, int>>>[] ResultOnFailureNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(EnsureOnFailureIsNotCalled, _ => { }),
            result => Task.FromResult(result).Switch(EnsureOnFailureIsNotCalled, () => { }),
            result => Task.FromResult(result).Switch(_ => EnsureOnFailureIsNotCalled(), _ => { }),
            result => Task.FromResult(result).Switch(_ => EnsureOnFailureIsNotCalled(), () => { }),
        };

        private static readonly Func<IResult<string, int>, Task<Result<string, int>>>[] InterfaceOnFailureNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(EnsureOnFailureIsNotCalled, _ => { }),
            result => Task.FromResult(result).Switch(EnsureOnFailureIsNotCalled, () => { }),
            result => Task.FromResult(result).Switch(_ => EnsureOnFailureIsNotCalled(), _ => { }),
            result => Task.FromResult(result).Switch(_ => EnsureOnFailureIsNotCalled(), () => { }),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallOnFailureIfSuccessCases =
            CreateDoNotCallCases(Result<string, int>.Succeed(5), ResultOnFailureNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<string, int>, string, int>(
                    Result<string, int>.Succeed(5),
                    InterfaceOnFailureNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnFailureIfSuccessCases))]
        public async Task Do_Not_Call_OnFailure_If_Success<TResult>(
            TResult result,
            Func<TResult, Task<Result<string, int>>> assertOnFailureIsNotCalled)
            where TResult : IResult<string, int>
        {
            await assertOnFailureIsNotCalled(result).ConfigureAwait(false);
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<TResult, Task<Result<int, string>>> method)
            where TResult : IResult<int, string>
        {
            return new(source, method) { ExpectedResult = source };
        }

        private static readonly Func<Result<int, string>, Task<Result<int, string>>>[] ResultReturnMethods =
        {
            result => Task.FromResult(result).Switch(_ => { }, _ => { }),
            result => Task.FromResult(result).Switch(_ => { }, () => { }),
            result => Task.FromResult(result).Switch(() => { }, _ => { }),
            result => Task.FromResult(result).Switch(() => { }, () => { }),
        };

        private static readonly Func<IResult<int, string>, Task<Result<int, string>>>[] InterfaceReturnMethods =
        {
            result => Task.FromResult(result).Switch(_ => { }, _ => { }),
            result => Task.FromResult(result).Switch(_ => { }, () => { }),
            result => Task.FromResult(result).Switch(() => { }, _ => { }),
            result => Task.FromResult(result).Switch(() => { }, () => { }),
        };

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<int, string>>>> methods)
            where TResult : IResult<int, string>
        {
            return methods.Select(method => CreateReturnCase(source, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            GenerateReturnCases(Result<int, string>.Fail(3), ResultReturnMethods)
                .Concat(GenerateReturnCases<IResult<int, string>>(Result<int, string>.Fail(3), InterfaceReturnMethods))
                .Concat(GenerateReturnCases(Result<int, string>.Succeed("yes"), ResultReturnMethods))
                .Concat(GenerateReturnCases<IResult<int, string>>(Result<int, string>.Succeed("yes"), InterfaceReturnMethods));

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Task<Result<int, string>> Return_Self<TResult>(
            TResult result,
            Func<TResult, Task<Result<int, string>>> callSwitch)
            where TResult : IResult<int, string>
        {
            return callSwitch(result);
        }
    }
}
