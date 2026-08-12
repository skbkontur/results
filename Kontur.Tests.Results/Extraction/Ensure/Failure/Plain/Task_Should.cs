using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.Plain
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly Func<Result<int>, ValueTask>[] ResultMethodsValueTasks =
        {
            result => ValueTaskFactory.Create(result).EnsureFailure(() => new MyException()),
            result => result.EnsureFailure(() => ValueTaskFactory.Create<Exception>(new MyException())),
        };

        private static readonly Func<Result<int>, Task>[] ResultMethodsTasks =
        {
            result => Task.FromResult(result).EnsureFailure(() => new MyException()),
            result => Task.FromResult(result).EnsureFailure(new MyException()),
            result => Task.FromResult(result).EnsureFailure(() => Task.FromResult<Exception>(new MyException())),
            result => ValueTaskFactory.Create(result).EnsureFailure(() => Task.FromResult<Exception>(new MyException())),
        };

        private static IEnumerable<Func<Result<int>, Task>> ResultMethods()
        {
            foreach (var method in ResultMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in ResultMethodsValueTasks)
            {
                yield return async result => await method(result).ConfigureAwait(false);
            }
        }

        private static readonly Func<IResult<int>, Task>[] InterfaceMethods =
        {
            result => Task.FromResult(result).EnsureFailure(() => new MyException()),
            result => Task.FromResult(result).EnsureFailure(new MyException()),
        };

        private static IEnumerable<TestCaseData> CreateCases<TResult>(
            TResult result,
            IEnumerable<Func<TResult, Task>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => new TestCaseData(result, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            CreateCases(Result<int>.Succeed(), ResultMethods())
                .Concat(CreateCases<IResult<int>>(Result<int>.Succeed(), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public async Task Throw_MyException_If_Success<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<int>
        {
            Func<Task> action = () => callEnsure(result);

            await action.Should().ThrowAsync<MyException>().ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> NotThrowCases =
            CreateCases(Result<int>.Fail(1), ResultMethods().Append(result => Task.FromResult(result).EnsureFailure()))
                .Concat(CreateCases<IResult<int>>(
                    Result<int>.Fail(1),
                    InterfaceMethods.Append(result => Task.FromResult(result).EnsureFailure())));

        [TestCaseSource(nameof(NotThrowCases))]
        public async Task Do_Not_Throw_If_Failure<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<int>
        {
            await callEnsure(result).ConfigureAwait(false);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        private static readonly Func<Result<int>, Task>[] ResultAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureFailure(AssertIsNotCalled),
        };

        private static readonly Func<IResult<int>, Task>[] InterfaceAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureFailure(AssertIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryCases =
            CreateCases(Result<int>.Fail(5), ResultAssertNotCalledMethods)
                .Concat(CreateCases<IResult<int>>(Result<int>.Fail(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryCases))]
        public async Task Do_No_Create_Exception_If_Success<TResult>(TResult result, Func<TResult, Task> assertExtracted)
            where TResult : IResult<int>
        {
            await assertExtracted(result).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
