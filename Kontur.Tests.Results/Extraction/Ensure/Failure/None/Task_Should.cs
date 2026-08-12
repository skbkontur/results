using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.None
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly Func<Optional<int>, ValueTask>[] OptionalMethodsValueTasks =
        {
            optional => ValueTaskFactory.Create(optional).EnsureNone(_ => new MyException()),
            optional => optional.EnsureNone(() => ValueTaskFactory.Create<Exception>(new MyException())),
        };

        private static readonly Func<Optional<int>, Task>[] OptionalMethodsTasks =
        {
            optional => Task.FromResult(optional).EnsureNone(_ => new MyException()),
            optional => Task.FromResult(optional).EnsureNone(() => new MyException()),
            optional => Task.FromResult(optional).EnsureNone(new MyException()),
            optional => Task.FromResult(optional).EnsureNone(_ => Task.FromResult<Exception>(new MyException())),
            optional => ValueTaskFactory.Create(optional).EnsureNone(() => Task.FromResult<Exception>(new MyException())),
        };

        private static IEnumerable<Func<Optional<int>, Task>> OptionalMethods()
        {
            foreach (var method in OptionalMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in OptionalMethodsValueTasks)
            {
                yield return async optional => await method(optional).ConfigureAwait(false);
            }
        }

        private static readonly Func<IOptional<int>, Task>[] InterfaceMethods =
        {
            optional => Task.FromResult(optional).EnsureNone(_ => new MyException()),
            optional => Task.FromResult(optional).EnsureNone(() => new MyException()),
            optional => Task.FromResult(optional).EnsureNone(new MyException()),
        };

        private static IEnumerable<TestCaseData> CreateCases<TOptional>(
            TOptional optional,
            IEnumerable<Func<TOptional, Task>> methods)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => new TestCaseData(optional, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            CreateCases(Optional<int>.Some(5), OptionalMethods())
                .Concat(CreateCases<IOptional<int>>(Optional<int>.Some(5), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public async Task Throw_MyException_If_Some<TOptional>(TOptional optional, Func<TOptional, Task> callEnsure)
            where TOptional : IOptional<int>
        {
            Func<Task> action = () => callEnsure(optional);

            await action.Should().ThrowAsync<MyException>().ConfigureAwait(false);
        }

        private static TestCaseData CreatePassValueCase<TOptional>(
            Func<int, TOptional> factory,
            Func<TOptional, Func<int, Exception>, Task> callEnsure)
            where TOptional : IOptional<int>
        {
            return new(factory, callEnsure);
        }

        private static readonly TestCaseData[] PassValueCases =
        {
            CreatePassValueCase(Optional<int>.Some, (optional, map) => Task.FromResult(optional).EnsureNone(value => map(value))),
            CreatePassValueCase<IOptional<int>>(Optional<int>.Some, (optional, map) => Task.FromResult(optional).EnsureNone(value => map(value))),
        };

        [TestCaseSource(nameof(PassValueCases))]
        public async Task Pass_Value_To_Exception_Factory<TOptional>(
            Func<int, TOptional> factory,
            Func<TOptional, Func<int, Exception>, Task> callEnsure)
            where TOptional : IOptional<int>
        {
            var source = factory(777);

            Func<Task> action = () => callEnsure(source, value => new Exception(value.ToString(CultureInfo.InvariantCulture)));

            (await action.Should().ThrowAsync<Exception>().ConfigureAwait(false))
                .WithMessage("777");
        }

        private static readonly IEnumerable<TestCaseData> NotThrowCases =
            CreateCases(Optional<int>.None(), OptionalMethods().Append(optional => Task.FromResult(optional).EnsureNone()))
                .Concat(CreateCases<IOptional<int>>(
                    Optional<int>.None(),
                    InterfaceMethods.Append(optional => Task.FromResult(optional).EnsureNone())));

        [TestCaseSource(nameof(NotThrowCases))]
        public async Task Do_Not_Throw_If_None<TOptional>(TOptional optional, Func<TOptional, Task> callEnsure)
            where TOptional : IOptional<int>
        {
            await callEnsure(optional).ConfigureAwait(false);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on none");
            throw new UnreachableException();
        }

        private static readonly Func<Optional<int>, Task>[] OptionalAssertNotCalledMethods =
        {
            optional => Task.FromResult(optional).EnsureNone(_ => AssertIsNotCalled()),
            optional => Task.FromResult(optional).EnsureNone(AssertIsNotCalled),
        };

        private static readonly Func<IOptional<int>, Task>[] InterfaceAssertNotCalledMethods =
        {
            optional => Task.FromResult(optional).EnsureNone(_ => AssertIsNotCalled()),
            optional => Task.FromResult(optional).EnsureNone(AssertIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryCases =
            CreateCases(Optional<int>.None(), OptionalAssertNotCalledMethods)
                .Concat(CreateCases<IOptional<int>>(Optional<int>.None(), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryCases))]
        public async Task Do_No_Create_Exception_If_Failure<TOptional>(TOptional optional, Func<TOptional, Task> assertExtracted)
            where TOptional : IOptional<int>
        {
            await assertExtracted(optional).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
