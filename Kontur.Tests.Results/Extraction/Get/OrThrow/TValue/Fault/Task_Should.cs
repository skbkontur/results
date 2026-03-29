using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Fault
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateThrowCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int>> method)
            where TResult : IResult<int, string>
        {
            return new(source, method);
        }

        private static readonly Func<Task<Result<int, string>>, Task<int>>[] ResultMethods =
        {
            result => result.GetFaultOrThrow(_ => new MyException()),
            result => result.GetFaultOrThrow(() => new MyException()),
            result => result.GetFaultOrThrow(new MyException()),
        };

        private static readonly Func<Task<IResult<int, string>>, Task<int>>[] InterfaceMethods =
        {
            result => result.GetFaultOrThrow(_ => new MyException()),
            result => result.GetFaultOrThrow(() => new MyException()),
            result => result.GetFaultOrThrow(new MyException()),
        };

        private static IEnumerable<TestCaseData> GenerateThrowCases<TResult>(
            TResult success,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<int, string>
        {
            return methods.Select(method => CreateThrowCase(success, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            GenerateThrowCases(Result<int, string>.Succeed("val"), ResultMethods)
                .Concat(GenerateThrowCases(Result<int, string>.Succeed("val"), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public void Throw_MyException_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<int>> callGetOrThrow)
            where TResult : IResult<int, string>
        {
            var task = Task.FromResult(result);

            Func<Task> action = () => callGetOrThrow(task);

            action.Should().ThrowAsync<MyException>();
        }

        private static TestCaseData CreateReturnCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int>> method)
            where TResult : IResult<int, string>
        {
            return new(source, method)
            {
                ExpectedResult = 5,
            };
        }

        private static IEnumerable<TestCaseData> GenerateReturnCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<int, string>
        {
            return methods.Select(method => CreateReturnCase(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnCases =
            GenerateReturnCases(Result<int, string>.Fail(5), ResultMethods)
                .Concat(GenerateReturnCases(Result<int, string>.Fail(5), InterfaceMethods));

        [TestCaseSource(nameof(ReturnCases))]
        public Task<int> Return_Fault_If_Failure<TResult>(TResult result, Func<Task<TResult>, Task<int>> callGetOrThrow)
            where TResult : IResult<int, string>
        {
            var task = Task.FromResult(result);

            return callGetOrThrow(task);
        }

        private static TestCaseData CreatePassValueCase<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, Exception>, Task<string>> callGetOrThrow)
            where TResult : IResult<string, int>
        {
            return new(factory, callGetOrThrow);
        }

        private static readonly TestCaseData[] PassValueCases =
        {
            CreatePassValueCase(Result<string, int>.Succeed, (result, map) => result.GetFaultOrThrow(value => map(value))),
            CreatePassValueCase<IResult<string, int>>(Result<string, int>.Succeed, (result, map) => result.GetFaultOrThrow(value => map(value))),
        };

        [TestCaseSource(nameof(PassValueCases))]
        public async Task Pass_Value_To_Exception_Factory<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, Exception>, Task<string>> callGetOrThrow)
            where TResult : IResult<string, int>
        {
            var source = Task.FromResult(factory(777));

            Func<Task> action = () => callGetOrThrow(source, i => new Exception(i.ToString(CultureInfo.InvariantCulture)));

            (await action.Should().ThrowAsync<Exception>().ConfigureAwait(false))
                .WithMessage("777");
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on Failure");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<int, string>>, Task<int>>[] ResultAssertNotCalledMethods =
        {
            result => result.GetFaultOrThrow(_ => AssertIsNotCalled()),
            result => result.GetFaultOrThrow(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<int, string>>, Task<int>>[] InterfaceAssertNotCalledMethods =
        {
            result => result.GetFaultOrThrow(_ => AssertIsNotCalled()),
            result => result.GetFaultOrThrow(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<int>>> methods)
            where TResult : IResult<int, string>
        {
            return methods.Select(method => new TestCaseData(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfFailureCases =
            CreateDoNotCallFactoryCases(Result<int, string>.Fail(5), ResultAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Result<int, string>.Fail(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfFailureCases))]
        public async Task Do_Not_Call_Delegate_If_Failure<TResult>(TResult result, Func<Task<TResult>, Task<int>> assertExtracted)
            where TResult : IResult<int, string>
        {
            var task = Task.FromResult(result);

            await assertExtracted(task).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
