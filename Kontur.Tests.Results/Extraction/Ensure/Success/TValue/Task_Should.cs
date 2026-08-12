using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.TValue
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly Func<Result<string, int>, ValueTask>[] ResultMethodsValueTasks =
        {
            result => ValueTaskFactory.Create(result).EnsureSuccess(_ => new MyException()),
            result => result.EnsureSuccess(() => ValueTaskFactory.Create<Exception>(new MyException())),
        };

        private static readonly Func<Result<string, int>, Task>[] ResultMethodsTasks =
        {
            result => Task.FromResult(result).EnsureSuccess(_ => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(() => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(new MyException()),
            result => Task.FromResult(result).EnsureSuccess(_ => Task.FromResult<Exception>(new MyException())),
            result => ValueTaskFactory.Create(result).EnsureSuccess(() => Task.FromResult<Exception>(new MyException())),
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
            result => Task.FromResult(result).EnsureSuccess(_ => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(() => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(new MyException()),
        };

        private static IEnumerable<TestCaseData> CreateCases<TResult>(
            TResult result,
            IEnumerable<Func<TResult, Task>> methods)
            where TResult : IResult<string, int>
        {
            return methods.Select(method => new TestCaseData(result, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            CreateCases(Result<string, int>.Fail("oops"), ResultMethods())
                .Concat(CreateCases<IResult<string, int>>(Result<string, int>.Fail("oops"), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public async Task Throw_MyException_If_Failure<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            Func<Task> action = () => callEnsure(result);

            await action.Should().ThrowAsync<MyException>().ConfigureAwait(false);
        }

        private static TestCaseData CreatePassFaultCase<TResult>(
            Func<string, TResult> factory,
            Func<TResult, Func<string, Exception>, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            return new(factory, callEnsure);
        }

        private static readonly TestCaseData[] PassFaultCases =
        {
            CreatePassFaultCase(Result<string, int>.Fail, (result, map) => Task.FromResult(result).EnsureSuccess(fault => map(fault))),
            CreatePassFaultCase<IResult<string, int>>(Result<string, int>.Fail, (result, map) => Task.FromResult(result).EnsureSuccess(fault => map(fault))),
        };

        [TestCaseSource(nameof(PassFaultCases))]
        public async Task Pass_Fault_To_Exception_Factory<TResult>(
            Func<string, TResult> factory,
            Func<TResult, Func<string, Exception>, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            const string expected = "example";
            var source = factory(expected);

            Func<Task> action = () => callEnsure(source, fault => new Exception(fault));

            (await action.Should().ThrowAsync<Exception>().ConfigureAwait(false))
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> NotThrowCases =
            CreateCases(
                    Result<string, int>.Succeed(5),
                    ResultMethods().Append(result => Task.FromResult(result).EnsureSuccess()))
                .Concat(CreateCases<IResult<string, int>>(
                    Result<string, int>.Succeed(5),
                    InterfaceMethods.Append(result => Task.FromResult(result).EnsureSuccess())));

        [TestCaseSource(nameof(NotThrowCases))]
        public async Task Do_Not_Throw_If_Success<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<string, int>
        {
            await callEnsure(result).ConfigureAwait(false);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on success");
            throw new UnreachableException();
        }

        private static readonly Func<Result<string, int>, Task>[] ResultAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureSuccess(_ => AssertIsNotCalled()),
            result => Task.FromResult(result).EnsureSuccess(AssertIsNotCalled),
        };

        private static readonly Func<IResult<string, int>, Task>[] InterfaceAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureSuccess(_ => AssertIsNotCalled()),
            result => Task.FromResult(result).EnsureSuccess(AssertIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryCases =
            CreateCases(Result<string, int>.Succeed(5), ResultAssertNotCalledMethods)
                .Concat(CreateCases<IResult<string, int>>(Result<string, int>.Succeed(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryCases))]
        public async Task Do_No_Create_Exception_If_Success<TResult>(TResult result, Func<TResult, Task> assertExtracted)
            where TResult : IResult<string, int>
        {
            await assertExtracted(result).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
