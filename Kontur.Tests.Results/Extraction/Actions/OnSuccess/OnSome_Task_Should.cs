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
    internal class OnSome_Task_Should
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
            (optional, counter) => ValueTaskFactory.Create(optional).OnSome(counter.Increment),
            (optional, counter) => optional.OnSome(_ => IncrementValueTask(counter)),
        };

        private static readonly Func<Optional<string>, ICounter, Task<Optional<string>>>[] CallMethodsTasks =
        {
            (optional, counter) => Task.FromResult(optional).OnSome(counter.Increment),
            (optional, counter) => Task.FromResult(optional).OnSome(_ => counter.Increment()),
            (optional, counter) => Task.FromResult(optional).OnSome(_ => IncrementTask(counter)),
            (optional, counter) => ValueTaskFactory.Create(optional).OnSome(_ => IncrementTask(counter)),
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

        private static readonly IEnumerable<TestCaseData> CallOnSomeIfSomeCases =
            CallMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(CallOnSomeIfSomeCases))]
        public async Task Call_OnSome_If_Some(
            Func<Optional<string>, ICounter, Task<Optional<string>>> callOnSome)
        {
            var counter = Substitute.For<ICounter>();

            await callOnSome(Optional<string>.Some("foo"), counter).ConfigureAwait(false);

            counter.Received().Increment();
        }

        [Test]
        public async Task Pass_Value_If_Some()
        {
            const string expected = "foo";
            var consumer = Substitute.For<IConsumer>();

            await Task.FromResult(Optional<string>.Some(expected))
                .OnSome(value => consumer.Consume(value))
                .ConfigureAwait(false);

            consumer.Received().Consume(expected);
        }

        private static void EnsureOnSomeIsNotCalled()
        {
            Assert.Fail("OnSome is called");
        }

        private static readonly Func<Optional<string>, Task<Optional<string>>>[] OptionalNotCalledMethods =
        {
            optional => Task.FromResult(optional).OnSome(_ => EnsureOnSomeIsNotCalled()),
            optional => Task.FromResult(optional).OnSome(EnsureOnSomeIsNotCalled),
        };

        private static readonly Func<IOptional<string>, Task<Optional<string>>>[] InterfaceNotCalledMethods =
        {
            optional => Task.FromResult(optional).OnSome(_ => EnsureOnSomeIsNotCalled()),
            optional => Task.FromResult(optional).OnSome(EnsureOnSomeIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallCases<TOptional>(
            TOptional none,
            IEnumerable<Func<TOptional, Task<Optional<string>>>> methods)
            where TOptional : IOptional<string>
        {
            return methods.Select(method => new TestCaseData(none, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallOnSomeIfNoneCases =
            CreateDoNotCallCases(Optional<string>.None(), OptionalNotCalledMethods)
                .Concat(CreateDoNotCallCases<IOptional<string>>(Optional<string>.None(), InterfaceNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallOnSomeIfNoneCases))]
        public async Task Do_Not_Call_OnSome_If_None<TOptional>(
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
            optional => Task.FromResult(optional).OnSome(_ => { }),
            optional => Task.FromResult(optional).OnSome(() => { }),
        };

        private static readonly Func<IOptional<int>, Task<Optional<int>>>[] InterfaceReturnMethods =
        {
            optional => Task.FromResult(optional).OnSome(_ => { }),
            optional => Task.FromResult(optional).OnSome(() => { }),
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
            Func<TOptional, Task<Optional<int>>> callOnSome)
            where TOptional : IOptional<int>
        {
            return callOnSome(optional);
        }
    }
}
