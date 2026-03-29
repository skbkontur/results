using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Plain
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateThrowCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int>> method)
            where TResult : IResult<int>
        {
            return new(source, method);
        }

        private static readonly Func<Task<Result<int>>, Task<int>>[] ResultMethods =
        {
            result => result.GetFaultOrThrow(() => new MyException()),
            result => result.GetFaultOrThrow(new MyException()),
        };

        private static readonly Func<Task<IResult<int>>, Task<int>>[] InterfaceMethods =
        {
            result => result.GetFaultOrThrow(() => new MyException()),
            result => result.GetFaultOrThrow(new MyException()),
        };

        private static IEnumerable<TestCaseData> GenerateThrowCases<TResult>(
            TResult success,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => CreateThrowCase(success, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            GenerateThrowCases(Result<int>.Succeed(), ResultMethods)
                .Concat(GenerateThrowCases(Result<int>.Succeed(), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public void Throw_MyException_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<int>> callGetOrThrow)
            where TResult : IResult<int>
        {
            var task = Task.FromResult(result);

            Func<Task> action = () => callGetOrThrow(task);

            action.Should().ThrowAsync<MyException>();
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int>> method)
            where TResult : IResult<int>
        {
            return new(source, method)
            {
                ExpectedResult = 5,
            };
        }

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => CreateReturnCase(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnCases =
            GenerateReturnCases(Result<int>.Fail(5), ResultMethods)
                .Concat(GenerateReturnCases(Result<int>.Fail(5), InterfaceMethods));

        [TestCaseSource(nameof(ReturnCases))]
        public Task<int> Return_Fault_If_Failure<TResult>(TResult result, Func<Task<TResult>, Task<int>> callGetOrThrow)
            where TResult : IResult<int>
        {
            var task = Task.FromResult(result);

            return callGetOrThrow(task);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on Failure");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<int>>, Task<int>>[] ResultAssertNotCalledMethods =
        {
            result => result.GetFaultOrThrow(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<int>>, Task<int>>[] InterfaceAssertNotCalledMethods =
        {
            result => result.GetFaultOrThrow(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<int>
        {
            return methods.Select(method => new TestCaseData(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfFailureCases =
            CreateDoNotCallFactoryCases(Result<int>.Fail(5), ResultAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Result<int>.Fail(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfFailureCases))]
        public async Task Do_Not_Call_Delegate_If_Failure<TResult>(TResult result, Func<Task<TResult>, Task<int>> assertExtracted)
            where TResult : IResult<int>
        {
            var task = Task.FromResult(result);

            await assertExtracted(task).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
