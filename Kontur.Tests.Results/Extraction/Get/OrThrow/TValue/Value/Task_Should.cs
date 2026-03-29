using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Value
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateThrowCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int>> method)
            where TResult : IResult<string, int>
        {
            return new(source, method);
        }

        private static readonly Func<Task<Result<string, int>>, Task<int>>[] ResultMethods =
        {
            result => result.GetValueOrThrow(_ => new MyException()),
            result => result.GetValueOrThrow(() => new MyException()),
            result => result.GetValueOrThrow(new MyException()),
        };

        private static readonly Func<Task<IResult<string, int>>, Task<int>>[] InterfaceMethods =
        {
            result => result.GetValueOrThrow(_ => new MyException()),
            result => result.GetValueOrThrow(() => new MyException()),
            result => result.GetValueOrThrow(new MyException()),
        };

        private static IEnumerable<TestCaseData> GenerateThrowCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<string, int>
        {
            return methods.Select(method => CreateThrowCase(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            GenerateThrowCases(Result<string, int>.Fail("err"), ResultMethods)
                .Concat(GenerateThrowCases(Result<string, int>.Fail("err"), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public void Throw_MyException_If_Failure<TResult>(TResult result, Func<Task<TResult>, Task<int>> callGetOrThrow)
            where TResult : IResult<string, int>
        {
            var task = Task.FromResult(result);

            Func<Task> action = () => callGetOrThrow(task);

            action.Should().ThrowAsync<MyException>();
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int>> method)
            where TResult : IResult<string, int>
        {
            return new(source, method)
            {
                ExpectedResult = 5,
            };
        }

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult success,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<string, int>
        {
            return methods.Select(method => CreateReturnCase(success, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnCases =
            GenerateReturnCases(Result<string, int>.Succeed(5), ResultMethods)
                .Concat(GenerateReturnCases(Result<string, int>.Succeed(5), InterfaceMethods));

        [TestCaseSource(nameof(ReturnCases))]
        public Task<int> Return_Value_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<int>> callGetOrThrow)
            where TResult : IResult<string, int>
        {
            var task = Task.FromResult(result);

            return callGetOrThrow(task);
        }

        private static TestCaseData CreatePassFaultCase<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, Exception>, Task<string>> callGetOrThrow)
            where TResult : IResult<int, string>
        {
            return new(factory, callGetOrThrow);
        }

        private static readonly TestCaseData[] PassFaultCases =
        {
            CreatePassFaultCase(Result<int, string>.Fail, (result, map) => result.GetValueOrThrow(fault => map(fault))),
            CreatePassFaultCase<IResult<int, string>>(Result<int, string>.Fail, (result, map) => result.GetValueOrThrow(fault => map(fault))),
        };

        [TestCaseSource(nameof(PassFaultCases))]
        public async Task Pass_Fault_To_Exception_Factory<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, Exception>, Task<string>> callGetOrThrow)
            where TResult : IResult<int, string>
        {
            var source = Task.FromResult(factory(777));

            Func<Task> action = () => callGetOrThrow(source, i => new Exception(i.ToString(CultureInfo.InvariantCulture)));

            (await action.Should().ThrowAsync<Exception>().ConfigureAwait(false))
                .WithMessage("777");
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on Success");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<string, int>>, Task<int>>[] ResultAssertNotCalledMethods =
        {
            result => result.GetValueOrThrow(_ => AssertIsNotCalled()),
            result => result.GetValueOrThrow(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<string, int>>, Task<int>>[] InterfaceAssertNotCalledMethods =
        {
            result => result.GetValueOrThrow(_ => AssertIsNotCalled()),
            result => result.GetValueOrThrow(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TResult>(
            TResult success,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<string, int>
        {
            return methods.Select(method => new TestCaseData(success, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfSuccessCases =
            CreateDoNotCallFactoryCases(Result<string, int>.Succeed(5), ResultAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Result<string, int>.Succeed(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfSuccessCases))]
        public async Task Do_Not_Call_Delegate_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<int>> assertExtracted)
            where TResult : IResult<string, int>
        {
            var task = Task.FromResult(result);

            await assertExtracted(task).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
