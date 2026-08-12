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
    internal class Optional_Task_Should
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

        private static IEnumerable<Func<Optional<string>, ICounter, Task<Optional<string>>>> CreateCallMethods(
            IEnumerable<Func<Optional<string>, ICounter, Task<Optional<string>>>> tasks,
            IEnumerable<Func<Optional<string>, ICounter, ValueTask<Optional<string>>>> valueTasks)
        {
            foreach (var method in tasks)
            {
                yield return method;
            }

            foreach (var method in valueTasks)
            {
                yield return async (optional, counter) => await method(optional, counter).ConfigureAwait(false);
            }
        }

        private static readonly Func<Optional<string>, ICounter, ValueTask<Optional<string>>>[] CallOnSomeMethodsValueTasks =
        {
            (optional, counter) => ValueTaskFactory.Create(optional).Switch(() => { }, counter.Increment),
            (optional, counter) => optional.Switch(() => { }, _ => IncrementValueTask(counter)),
        };

        private static readonly Func<Optional<string>, ICounter, Task<Optional<string>>>[] CallOnSomeMethodsTasks =
        {
            (optional, counter) => Task.FromResult(optional).Switch(() => { }, _ => counter.Increment()),
            (optional, counter) => Task.FromResult(optional).Switch(() => { }, counter.Increment),
            (optional, counter) => Task.FromResult(optional).Switch(() => { }, _ => IncrementTask(counter)),
            (optional, counter) => ValueTaskFactory.Create(optional).Switch(() => { }, _ => IncrementTask(counter)),
        };

        private static readonly IEnumerable<TestCaseData> CallOnSomeIfSomeCases =
            CreateCallMethods(CallOnSomeMethodsTasks, CallOnSomeMethodsValueTasks)
                .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnSomeIfSomeCases))]
        public async Task Call_OnSome_If_Some(
            Func<Optional<string>, ICounter, Task<Optional<string>>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();

            await callSwitch(Optional<string>.Some("foo"), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        [Test]
        public async Task Pass_Value_OnSome()
        {
            const string expected = "foo";
            var consumer = Substitute.For<IConsumer>();

            await Task.FromResult(Optional<string>.Some(expected))
                .Switch(() => { }, value => consumer.Consume(value))
                .ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static readonly Func<Optional<string>, ICounter, ValueTask<Optional<string>>>[] CallOnNoneMethodsValueTasks =
        {
            (optional, counter) => ValueTaskFactory.Create(optional).Switch(counter.Increment, _ => { }),
            (optional, counter) => optional.Switch(() => IncrementValueTask(counter), _ => { }),
        };

        private static readonly Func<Optional<string>, ICounter, Task<Optional<string>>>[] CallOnNoneMethodsTasks =
        {
            (optional, counter) => Task.FromResult(optional).Switch(counter.Increment, _ => { }),
            (optional, counter) => Task.FromResult(optional).Switch(counter.Increment, () => { }),
            (optional, counter) => Task.FromResult(optional).Switch(() => IncrementTask(counter), _ => { }),
            (optional, counter) => ValueTaskFactory.Create(optional).Switch(() => IncrementTask(counter), _ => { }),
        };

        private static readonly IEnumerable<TestCaseData> CallOnNoneIfNoneCases =
            CreateCallMethods(CallOnNoneMethodsTasks, CallOnNoneMethodsValueTasks)
                .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnNoneIfNoneCases))]
        public async Task Call_OnNone_If_None(
            Func<Optional<string>, ICounter, Task<Optional<string>>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();

            await callSwitch(Optional<string>.None(), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        private static void EnsureOnSomeIsNotCalled()
        {
            Assert.Fail("OnSome is called");
        }

        private static readonly Func<Optional<string>, Task<Optional<string>>>[] OptionalOnSomeNotCalledMethods =
        {
            optional => Task.FromResult(optional).Switch(() => { }, EnsureOnSomeIsNotCalled),
            optional => Task.FromResult(optional).Switch(() => { }, _ => EnsureOnSomeIsNotCalled()),
        };

        private static readonly Func<IOptional<string>, Task<Optional<string>>>[] InterfaceOnSomeNotCalledMethods =
        {
            optional => Task.FromResult(optional).Switch(() => { }, EnsureOnSomeIsNotCalled),
            optional => Task.FromResult(optional).Switch(() => { }, _ => EnsureOnSomeIsNotCalled()),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TOptional>(
            TOptional source,
            IEnumerable<Func<TOptional, Task<Optional<string>>>> methods)
            where TOptional : IOptional<string>
        {
            return methods.Select(method => new TestCaseData(source, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnSomeIfNoneCases =
            CreateDoNotCallCases(Optional<string>.None(), OptionalOnSomeNotCalledMethods)
                .Concat(CreateDoNotCallCases<IOptional<string>>(Optional<string>.None(), InterfaceOnSomeNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnSomeIfNoneCases))]
        public async Task Do_Not_Call_OnSome_If_None<TOptional>(
            TOptional optional,
            Func<TOptional, Task<Optional<string>>> assertOnSomeIsNotCalled)
            where TOptional : IOptional<string>
        {
            await assertOnSomeIsNotCalled(optional).ConfigureAwait(false);
        }

        private static void EnsureOnNoneIsNotCalled()
        {
            Assert.Fail("OnNone is called");
        }

        private static readonly Func<Optional<string>, Task<Optional<string>>>[] OptionalOnNoneNotCalledMethods =
        {
            optional => Task.FromResult(optional).Switch(EnsureOnNoneIsNotCalled, _ => { }),
            optional => Task.FromResult(optional).Switch(EnsureOnNoneIsNotCalled, () => { }),
        };

        private static readonly Func<IOptional<string>, Task<Optional<string>>>[] InterfaceOnNoneNotCalledMethods =
        {
            optional => Task.FromResult(optional).Switch(EnsureOnNoneIsNotCalled, _ => { }),
            optional => Task.FromResult(optional).Switch(EnsureOnNoneIsNotCalled, () => { }),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallOnNoneIfSomeCases =
            CreateDoNotCallCases(Optional<string>.Some("foo"), OptionalOnNoneNotCalledMethods)
                .Concat(CreateDoNotCallCases<IOptional<string>>(Optional<string>.Some("foo"), InterfaceOnNoneNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnNoneIfSomeCases))]
        public async Task Do_Not_Call_OnNone_If_Some<TOptional>(
            TOptional optional,
            Func<TOptional, Task<Optional<string>>> assertOnNoneIsNotCalled)
            where TOptional : IOptional<string>
        {
            await assertOnNoneIsNotCalled(optional).ConfigureAwait(false);
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
            optional => Task.FromResult(optional).Switch(() => { }, _ => { }),
            optional => Task.FromResult(optional).Switch(() => { }, () => { }),
        };

        private static readonly Func<IOptional<int>, Task<Optional<int>>>[] InterfaceReturnMethods =
        {
            optional => Task.FromResult(optional).Switch(() => { }, _ => { }),
            optional => Task.FromResult(optional).Switch(() => { }, () => { }),
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
        public Task<Optional<int>> Return_Self<TOptional>(
            TOptional optional,
            Func<TOptional, Task<Optional<int>>> callSwitch)
            where TOptional : IOptional<int>
        {
            return callSwitch(optional);
        }
    }
}
