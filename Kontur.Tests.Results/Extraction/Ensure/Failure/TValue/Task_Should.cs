using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.TValue
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly Func<Result<string, int>, ValueTask>[] ResultMethodsValueTasks =
        {
            result => ValueTaskFactory.Create(result).EnsureFailure(_ => new MyException()),
            result => result.EnsureFailure(() => ValueTaskFactory.Create<Exception>(new MyException())),
        };

        private static readonly Func<Result<string, int>, Task>[] ResultMethodsTasks =
        {
            result => Task.FromResult(result).EnsureFailure(_ => new MyException()),
            result => Task.FromResult(result).EnsureFailure(() => new MyException()),
            result => Task.FromResult(result).EnsureFailure(new MyException()),
            result => Task.FromResult(result).EnsureFailure(_ => Task.FromResult<Exception>(new MyException())),
            result => ValueTaskFactory.Create(result).EnsureFailure(() => Task.FromResult<Exception>(new MyException())),
        };

        private static IEnumerable<Func<Result<string, int>, Task>> ResultMethods()
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

        private static readonly Func<IResult<string, int>, Task>[] InterfaceMethods =
        {
            result => Task.FromResult(result).EnsureFailure(_ => new MyException()),
            result => Task.FromResult(result).EnsureFailure(() => new MyException()),
            result => Task.FromResult(result).EnsureFailure(new MyException()),
        };

        private static IEnumerable<TestCaseData> CreateCases<TResult>(
            TResult result,
            IEnumerable<Func<TResult, Task>> methods)
            where TResult : IResult<string, int>
        {
            return methods.Select(method => new TestCaseData(result, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            CreateCases(Result<string, int>.Succeed(5), ResultMethods())
                .Concat(CreateCases<IResult<string, int>>(Result<string, int>.Succeed(5), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public async Task Throw_MyException_If_Success<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            Func<Task> action = () => callEnsure(result);

            await action.Should().ThrowAsync<MyException>().ConfigureAwait(false);
        }

        private static TestCaseData CreatePassValueCase<TResult>(
            Func<int, TResult> factory,
            Func<TResult, Func<int, Exception>, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            return new(factory, callEnsure);
        }

        private static readonly TestCaseData[] PassValueCases =
        {
            CreatePassValueCase(Result<string, int>.Succeed, (result, map) => Task.FromResult(result).EnsureFailure(value => map(value))),
            CreatePassValueCase<IResult<string, int>>(Result<string, int>.Succeed, (result, map) => Task.FromResult(result).EnsureFailure(value => map(value))),
        };

        [TestCaseSource(nameof(PassValueCases))]
        public async Task Pass_Value_To_Exception_Factory<TResult>(
            Func<int, TResult> factory,
            Func<TResult, Func<int, Exception>, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            var source = factory(777);

            Func<Task> action = () => callEnsure(source, value => new Exception(value.ToString(CultureInfo.InvariantCulture)));

            (await action.Should().ThrowAsync<Exception>().ConfigureAwait(false))
                .WithMessage("777");
        }

        private static readonly IEnumerable<TestCaseData> NotThrowCases =
            CreateCases(
                    Result<string, int>.Fail("fail"),
                    ResultMethods().Append(result => Task.FromResult(result).EnsureFailure()))
                .Concat(CreateCases<IResult<string, int>>(
                    Result<string, int>.Fail("fail"),
                    InterfaceMethods.Append(result => Task.FromResult(result).EnsureFailure())));

        [TestCaseSource(nameof(NotThrowCases))]
        public async Task Do_Not_Throw_If_Failure<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            await callEnsure(result).ConfigureAwait(false);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        private static readonly Func<Result<string, int>, Task>[] ResultAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureFailure(_ => AssertIsNotCalled()),
            result => Task.FromResult(result).EnsureFailure(AssertIsNotCalled),
        };

        private static readonly Func<IResult<string, int>, Task>[] InterfaceAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureFailure(_ => AssertIsNotCalled()),
            result => Task.FromResult(result).EnsureFailure(AssertIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryCases =
            CreateCases(Result<string, int>.Fail("fail"), ResultAssertNotCalledMethods)
                .Concat(CreateCases<IResult<string, int>>(Result<string, int>.Fail("fail"), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryCases))]
        public async Task Do_No_Create_Exception_If_Failure<TResult>(TResult result, Func<TResult, Task> assertExtracted)
            where TResult : IResult<string, int>
        {
            await assertExtracted(result).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
