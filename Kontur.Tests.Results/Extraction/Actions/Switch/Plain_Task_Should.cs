using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.Switch
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

        private static IEnumerable<Func<Result<int>, ICounter, Task<Result<int>>>> CreateCallMethods(
            IEnumerable<Func<Result<int>, ICounter, Task<Result<int>>>> tasks,
            IEnumerable<Func<Result<int>, ICounter, ValueTask<Result<int>>>> valueTasks)
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

        private static readonly Func<Result<int>, ICounter, ValueTask<Result<int>>>[] CallOnFailureMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).Switch(counter.Increment, () => { }),
            (result, counter) => result.Switch(_ => IncrementValueTask(counter), () => { }),
        };

        private static readonly Func<Result<int>, ICounter, Task<Result<int>>>[] CallOnFailureMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).Switch(_ => counter.Increment(), () => { }),
            (result, counter) => Task.FromResult(result).Switch(counter.Increment, () => { }),
            (result, counter) => Task.FromResult(result).Switch(_ => IncrementTask(counter), () => { }),
            (result, counter) => ValueTaskFactory.Create(result).Switch(_ => IncrementTask(counter), () => { }),
        };

        private static readonly IEnumerable<TestCaseData> CallOnFailureIfFailureCases =
            CreateCallMethods(CallOnFailureMethodsTasks, CallOnFailureMethodsValueTasks)
                .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public async Task Call_OnFailure_If_Failure(
            Func<Result<int>, ICounter, Task<Result<int>>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();

            await callSwitch(Result<int>.Fail(20), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        [Test]
        public async Task Pass_Fault_OnFailure()
        {
            const string expected = "foo";
            var consumer = Substitute.For<IConsumer>();

            await Task.FromResult(Result<string>.Fail(expected))
                .Switch(value => consumer.Consume(value), () => { })
                .ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static readonly Func<Result<int>, ICounter, ValueTask<Result<int>>>[] CallOnSuccessMethodsValueTasks =
        {
            (result, counter) => ValueTaskFactory.Create(result).Switch(_ => { }, counter.Increment),
            (result, counter) => result.Switch(_ => { }, () => IncrementValueTask(counter)),
        };

        private static readonly Func<Result<int>, ICounter, Task<Result<int>>>[] CallOnSuccessMethodsTasks =
        {
            (result, counter) => Task.FromResult(result).Switch(_ => { }, counter.Increment),
            (result, counter) => Task.FromResult(result).Switch(() => { }, counter.Increment),
            (result, counter) => Task.FromResult(result).Switch(_ => { }, () => IncrementTask(counter)),
            (result, counter) => ValueTaskFactory.Create(result).Switch(_ => { }, () => IncrementTask(counter)),
        };

        private static readonly IEnumerable<TestCaseData> CallOnSuccessIfSuccessCases =
            CreateCallMethods(CallOnSuccessMethodsTasks, CallOnSuccessMethodsValueTasks)
                .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public async Task Call_OnSuccess_If_Success(
            Func<Result<int>, ICounter, Task<Result<int>>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();

            await callSwitch(Result<int>.Succeed(), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        private static void EnsureOnFailureIsNotCalled()
        {
            Assert.Fail("OnFailure is called");
        }

        private static readonly Func<Result<string>, Task<Result<string>>>[] ResultOnFailureNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(EnsureOnFailureIsNotCalled, () => { }),
            result => Task.FromResult(result).Switch(_ => EnsureOnFailureIsNotCalled(), () => { }),
        };

        private static readonly Func<IResult<string>, Task<Result<string>>>[] InterfaceOnFailureNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(EnsureOnFailureIsNotCalled, () => { }),
            result => Task.FromResult(result).Switch(_ => EnsureOnFailureIsNotCalled(), () => { }),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TResult>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<string>>>> methods)
            where TResult : IResult<string>
        {
            return methods.Select(method => new TestCaseData(source, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnFailureIfSuccessCases =
            CreateDoNotCallCases(Result<string>.Succeed(), ResultOnFailureNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<string>>(Result<string>.Succeed(), InterfaceOnFailureNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnFailureIfSuccessCases))]
        public async Task Do_Not_Call_OnFailure_If_Success<TResult>(
            TResult result,
            Func<TResult, Task<Result<string>>> assertOnFailureIsNotCalled)
            where TResult : IResult<string>
        {
            await assertOnFailureIsNotCalled(result).ConfigureAwait(false);
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly Func<Result<string>, Task<Result<string>>>[] ResultOnSuccessNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(_ => { }, EnsureOnSuccessIsNotCalled),
            result => Task.FromResult(result).Switch(() => { }, EnsureOnSuccessIsNotCalled),
        };

        private static readonly Func<IResult<string>, Task<Result<string>>>[] InterfaceOnSuccessNotCalledMethods =
        {
            result => Task.FromResult(result).Switch(_ => { }, EnsureOnSuccessIsNotCalled),
            result => Task.FromResult(result).Switch(() => { }, EnsureOnSuccessIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallOnSuccessIfFailureCases =
            CreateDoNotCallCases(Result<string>.Fail("foo"), ResultOnSuccessNotCalledMethods)
                .Concat(CreateDoNotCallCases<IResult<string>>(Result<string>.Fail("foo"), InterfaceOnSuccessNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public async Task Do_Not_Call_OnSuccess_If_Failure<TResult>(
            TResult result,
            Func<TResult, Task<Result<string>>> assertOnSuccessIsNotCalled)
            where TResult : IResult<string>
        {
            await assertOnSuccessIsNotCalled(result).ConfigureAwait(false);
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
            result => Task.FromResult(result).Switch(_ => { }, () => { }),
            result => Task.FromResult(result).Switch(() => { }, () => { }),
        };

        private static readonly Func<IResult<int>, Task<Result<int>>>[] InterfaceReturnMethods =
        {
            result => Task.FromResult(result).Switch(_ => { }, () => { }),
            result => Task.FromResult(result).Switch(() => { }, () => { }),
        };

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult source,
            IEnumerable<Func<TResult, Task<Result<int>>>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => CreateReturnCase(source, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            GenerateReturnCases(Result<int>.Fail(5), ResultReturnMethods)
                .Concat(GenerateReturnCases<IResult<int>>(Result<int>.Fail(5), InterfaceReturnMethods))
                .Concat(GenerateReturnCases(Result<int>.Succeed(), ResultReturnMethods))
                .Concat(GenerateReturnCases<IResult<int>>(Result<int>.Succeed(), InterfaceReturnMethods));

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Task<Result<int>> Return_Self<TResult>(
            TResult result,
            Func<TResult, Task<Result<int>>> callSwitch)
            where TResult : IResult<int>
        {
            return callSwitch(result);
        }
    }
}
