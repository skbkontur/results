using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnFailure
{
    [TestFixture]
    internal class OnNone_Task_Should
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

        private static readonly Func<Optional<string>, ICounter, ValueTask<Optional<string>>>[] CallMethodsValueTasks =
        {
            (optional, counter) => ValueTaskFactory.Create(optional).OnNone(counter.Increment),
            (optional, counter) => optional.OnNone(() => IncrementValueTask(counter)),
        };

        private static readonly Func<Optional<string>, ICounter, Task<Optional<string>>>[] CallMethodsTasks =
        {
            (optional, counter) => Task.FromResult(optional).OnNone(counter.Increment),
            (optional, counter) => Task.FromResult(optional).OnNone(() => IncrementTask(counter)),
            (optional, counter) => ValueTaskFactory.Create(optional).OnNone(() => IncrementTask(counter)),
        };

        private static IEnumerable<Func<Optional<string>, ICounter, Task<Optional<string>>>> CallMethods()
        {
            foreach (var method in CallMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in CallMethodsValueTasks)
            {
                yield return async (optional, counter) => await method(optional, counter).ConfigureAwait(false);
            }
        }

        private static readonly IEnumerable<TestCaseData> CallOnNoneIfNoneCases =
            CallMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnNoneIfNoneCases))]
        public async Task Call_OnNone_If_None(
            Func<Optional<string>, ICounter, Task<Optional<string>>> callOnNone)
        {
            var counter = Substitute.For<ICounter>();

            await callOnNone(Optional<string>.None(), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        private static void EnsureOnNoneIsNotCalled()
        {
            Assert.Fail("OnNone is called");
        }

        private static readonly Func<Optional<string>, Task<Optional<string>>>[] OptionalNotCalledMethods =
        {
            optional => Task.FromResult(optional).OnNone(EnsureOnNoneIsNotCalled),
        };

        private static readonly Func<IOptional<string>, Task<Optional<string>>>[] InterfaceNotCalledMethods =
        {
            optional => Task.FromResult(optional).OnNone(EnsureOnNoneIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TOptional>(
            TOptional some,
            IEnumerable<Func<TOptional, Task<Optional<string>>>> methods)
            where TOptional : IOptional<string>
        {
            return methods.Select(method => new TestCaseData(some, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnNoneIfSomeCases =
            CreateDoNotCallCases(Optional<string>.Some("foo"), OptionalNotCalledMethods)
                .Concat(CreateDoNotCallCases<IOptional<string>>(Optional<string>.Some("foo"), InterfaceNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnNoneIfSomeCases))]
        public async Task Do_Not_Call_OnNone_If_Some<TOptional>(
            TOptional optional,
            Func<TOptional, Task<Optional<string>>> assertNotCalled)
            where TOptional : IOptional<string>
        {
            await assertNotCalled(optional).ConfigureAwait(false);
        }

        private static TestCaseData CreateReturnCase<TOptional>(
            TOptional source,
            Func<TOptional, Task<Optional<int>>> method)
            where TOptional : IOptional<int>
        {
            return new(source, method) { ExpectedResult = source };
        }

        private static readonly Func<Optional<int>, Task<Optional<int>>>[] OptionalReturnMethods =
        {
            optional => Task.FromResult(optional).OnNone(() => { }),
        };

        private static readonly Func<IOptional<int>, Task<Optional<int>>>[] InterfaceReturnMethods =
        {
            optional => Task.FromResult(optional).OnNone(() => { }),
        };

        private static IEnumerable<TestCaseData> GenerateReturnCases<TOptional>(
            TOptional source,
            IEnumerable<Func<TOptional, Task<Optional<int>>>> methods)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => CreateReturnCase(source, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            GenerateReturnCases(Optional<int>.Some(5), OptionalReturnMethods)
                .Concat(GenerateReturnCases<IOptional<int>>(Optional<int>.Some(5), InterfaceReturnMethods))
                .Concat(GenerateReturnCases(Optional<int>.None(), OptionalReturnMethods))
                .Concat(GenerateReturnCases<IOptional<int>>(Optional<int>.None(), InterfaceReturnMethods));

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Task<Optional<int>> Return_Self_OnNone<TOptional>(
            TOptional optional,
            Func<TOptional, Task<Optional<int>>> callOnNone)
            where TOptional : IOptional<int>
        {
            return callOnNone(optional);
        }
    }
}
