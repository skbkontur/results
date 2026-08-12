using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.HasValue
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly Func<Optional<int>, ValueTask>[] OptionalMethodsValueTasks =
        {
            optional => ValueTaskFactory.Create(optional).EnsureHasValue(() => new MyException()),
            optional => optional.EnsureHasValue(() => ValueTaskFactory.Create<Exception>(new MyException())),
        };

        private static readonly Func<Optional<int>, Task>[] OptionalMethodsTasks =
        {
            optional => Task.FromResult(optional).EnsureHasValue(() => new MyException()),
            optional => Task.FromResult(optional).EnsureHasValue(new MyException()),
            optional => Task.FromResult(optional).EnsureHasValue(() => Task.FromResult<Exception>(new MyException())),
            optional => ValueTaskFactory.Create(optional).EnsureHasValue(() => Task.FromResult<Exception>(new MyException())),
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
            optional => Task.FromResult(optional).EnsureHasValue(() => new MyException()),
            optional => Task.FromResult(optional).EnsureHasValue(new MyException()),
        };

        private static IEnumerable<TestCaseData> CreateCases<TOptional>(
            TOptional optional,
            IEnumerable<Func<TOptional, Task>> methods)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => new TestCaseData(optional, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            CreateCases(Optional<int>.None(), OptionalMethods())
                .Concat(CreateCases<IOptional<int>>(Optional<int>.None(), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public async Task Throw_MyException_If_None<TOptional>(TOptional optional, Func<TOptional, Task> callEnsure)
            where TOptional : IOptional<int>
        {
            Func<Task> action = () => callEnsure(optional);

            await action.Should().ThrowAsync<MyException>().ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> NotThrowCases =
            CreateCases(Optional<int>.Some(5), OptionalMethods().Append(optional => Task.FromResult(optional).EnsureHasValue()))
                .Concat(CreateCases<IOptional<int>>(
                    Optional<int>.Some(5),
                    InterfaceMethods.Append(optional => Task.FromResult(optional).EnsureHasValue())));

        [TestCaseSource(nameof(NotThrowCases))]
        public async Task Do_Not_Throw_If_Some<TOptional>(TOptional optional, Func<TOptional, Task> callEnsure)
            where TOptional : IOptional<int>
        {
            await callEnsure(optional).ConfigureAwait(false);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on some");
            throw new UnreachableException();
        }

        private static readonly Func<Optional<int>, Task>[] OptionalAssertNotCalledMethods =
        {
            optional => Task.FromResult(optional).EnsureHasValue(AssertIsNotCalled),
        };

        private static readonly Func<IOptional<int>, Task>[] InterfaceAssertNotCalledMethods =
        {
            optional => Task.FromResult(optional).EnsureHasValue(AssertIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryCases =
            CreateCases(Optional<int>.Some(5), OptionalAssertNotCalledMethods)
                .Concat(CreateCases<IOptional<int>>(Optional<int>.Some(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryCases))]
        public async Task Do_No_Create_Exception_If_Some<TOptional>(TOptional optional, Func<TOptional, Task> assertExtracted)
            where TOptional : IOptional<int>
        {
            await assertExtracted(optional).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
