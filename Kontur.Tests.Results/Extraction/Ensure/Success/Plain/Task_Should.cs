using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.Plain
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly Func<Result<int>, ValueTask>[] ResultMethodsValueTasks =
        {
            result => ValueTaskFactory.Create(result).EnsureSuccess(_ => new MyException()),
            result => result.EnsureSuccess(() => ValueTaskFactory.Create<Exception>(new MyException())),
        };

        private static readonly Func<Result<int>, Task>[] ResultMethodsTasks =
        {
            result => Task.FromResult(result).EnsureSuccess(_ => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(() => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(new MyException()),
            result => Task.FromResult(result).EnsureSuccess(_ => Task.FromResult<Exception>(new MyException())),
            result => ValueTaskFactory.Create(result).EnsureSuccess(() => Task.FromResult<Exception>(new MyException())),
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
            result => Task.FromResult(result).EnsureSuccess(_ => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(() => new MyException()),
            result => Task.FromResult(result).EnsureSuccess(new MyException()),
        };

        private static IEnumerable<TestCaseData> CreateCases<TResult>(
            TResult result,
            IEnumerable<Func<TResult, Task>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => new TestCaseData(result, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            CreateCases(Result<int>.Fail(1), ResultMethods())
                .Concat(CreateCases<IResult<int>>(Result<int>.Fail(1), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public async Task Throw_MyException_If_Failure<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<int>
        {
            Func<Task> action = () => callEnsure(result);

            await action.Should().ThrowAsync<MyException>().ConfigureAwait(false);
        }

        private static TestCaseData CreatePassFaultCase<TResult>(
            Func<int, TResult> factory,
            Func<TResult, Func<int, Exception>, Task> callEnsure)
            where TResult : IResult<int>
        {
            return new(factory, callEnsure);
        }

        private static readonly TestCaseData[] PassFaultCases =
        {
            CreatePassFaultCase(Result<int>.Fail, (result, map) => Task.FromResult(result).EnsureSuccess(fault => map(fault))),
            CreatePassFaultCase<IResult<int>>(Result<int>.Fail, (result, map) => Task.FromResult(result).EnsureSuccess(fault => map(fault))),
        };

        [TestCaseSource(nameof(PassFaultCases))]
        public async Task Pass_Fault_To_Exception_Factory<TResult>(
            Func<int, TResult> factory,
            Func<TResult, Func<int, Exception>, Task> callEnsure)
            where TResult : IResult<int>
        {
            var source = factory(777);

            Func<Task> action = () => callEnsure(source, fault => new Exception(fault.ToString(CultureInfo.InvariantCulture)));

            (await action.Should().ThrowAsync<Exception>().ConfigureAwait(false))
                .WithMessage("777");
        }

        private static readonly IEnumerable<TestCaseData> NotThrowCases =
            CreateCases(Result<int>.Succeed(), ResultMethods().Append(result => Task.FromResult(result).EnsureSuccess()))
                .Concat(CreateCases<IResult<int>>(
                    Result<int>.Succeed(),
                    InterfaceMethods.Append(result => Task.FromResult(result).EnsureSuccess())));

        [TestCaseSource(nameof(NotThrowCases))]
        public async Task Do_Not_Throw_If_Succeed<TResult>(TResult result, Func<TResult, Task> callEnsure)
            where TResult : IResult<int>
        {
            await callEnsure(result).ConfigureAwait(false);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on success");
            throw new UnreachableException();
        }

        private static readonly Func<Result<int>, Task>[] ResultAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureSuccess(_ => AssertIsNotCalled()),
            result => Task.FromResult(result).EnsureSuccess(AssertIsNotCalled),
        };

        private static readonly Func<IResult<int>, Task>[] InterfaceAssertNotCalledMethods =
        {
            result => Task.FromResult(result).EnsureSuccess(_ => AssertIsNotCalled()),
            result => Task.FromResult(result).EnsureSuccess(AssertIsNotCalled),
        };

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryCases =
            CreateCases(Result<int>.Succeed(), ResultAssertNotCalledMethods)
                .Concat(CreateCases<IResult<int>>(Result<int>.Succeed(), InterfaceAssertNotCalledMethods));

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
