using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrElse.TValue
{
    [TestFixture]
    internal class Value_Task_Should
    {
        private const string ExpectedValue = "fallback";

        private static TestCaseData CreateConvertCase<TResult>(
            TResult source,
            string expectedResult,
            Func<Task<TResult>, Task<string>> method)
            where TResult : IResult<Guid, string>
        {
            return new(source, method)
            {
                ExpectedResult = expectedResult,
            };
        }

        private static readonly Func<Task<Result<Guid, string>>, Task<string>>[] ResultMethods =
        {
            result => result.GetValueOrElse(_ => ExpectedValue),
            result => result.GetValueOrElse(() => ExpectedValue),
            result => result.GetValueOrElse(ExpectedValue),
        };

        private static readonly Func<Task<IResult<Guid, string>>, Task<string>>[] InterfaceMethods =
        {
            result => result.GetValueOrElse(_ => ExpectedValue),
            result => result.GetValueOrElse(() => ExpectedValue),
            result => result.GetValueOrElse(ExpectedValue),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TResult>(
            TResult failure,
            Func<string, TResult> successFactory,
            IEnumerable<Func<Task<TResult>, Task<string>>> methods)
            where TResult : IResult<Guid, string>
        {
            const string someValue = "bar";
            return from testCase in new[]
                {
                    (Source: failure, Result: ExpectedValue),
                    (Source: successFactory(someValue), Result: someValue),
                }
                from method in methods
                select CreateConvertCase(testCase.Source, testCase.Result, method);
        }

        private static readonly IEnumerable<TestCaseData> ConvertCases =
            GenerateCases(Result<Guid, string>.Fail(Guid.Empty), Result<Guid, string>.Succeed, ResultMethods)
                .Concat(GenerateCases(Result<Guid, string>.Fail(Guid.Empty), Result<Guid, string>.Succeed, InterfaceMethods));

        [TestCaseSource(nameof(ConvertCases))]
        public Task<string> Return_Result<TResult>(TResult result, Func<Task<TResult>, Task<string>> callGetOrElse)
            where TResult : IResult<Guid, string>
        {
            var task = Task.FromResult(result);

            return callGetOrElse(task);
        }

        private static TestCaseData CreateUseFaultCase<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<string>> callGetOrElse)
            where TResult : IResult<int, string>
        {
            return new(factory, callGetOrElse);
        }

        private static readonly TestCaseData[] UseFaultCases =
        {
            CreateUseFaultCase(Result<int, string>.Fail, (result, map) => result.GetValueOrElse(map)),
            CreateUseFaultCase<IResult<int, string>>(Result<int, string>.Fail, (result, map) => result.GetValueOrElse(map)),
        };

        [TestCaseSource(nameof(UseFaultCases))]
        public async Task Pass_Fault_To_Factory<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<string>> callGetOrElse)
            where TResult : IResult<int, string>
        {
            var source = Task.FromResult(factory(777));

            var value = await callGetOrElse(source, i => i.ToString(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);

            value.Should().Be("777");
        }

        private static string AssertIsNotCalled()
        {
            Assert.Fail("Backup value factory should not be called on Success");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<Guid, string>>, Task<string>>[] ResultAssertNotCalledMethods =
        {
            result => result.GetValueOrElse(_ => AssertIsNotCalled()),
            result => result.GetValueOrElse(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<Guid, string>>, Task<string>>[] InterfaceAssertNotCalledMethods =
        {
            result => result.GetValueOrElse(_ => AssertIsNotCalled()),
            result => result.GetValueOrElse(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TResult>(
            TResult success,
            IEnumerable<Func<Task<TResult>, Task<string>>> methods)
            where TResult : IResult<Guid, string>
        {
            return methods.Select(method => new TestCaseData(success, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfSuccessCases =
            CreateDoNotCallFactoryCases(Result<Guid, string>.Succeed("value"), ResultAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Result<Guid, string>.Succeed("value"), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfSuccessCases))]
        public async Task Do_Not_Call_Delegate_If_Success<TResult>(TResult result, Func<Task<TResult>, Task<string>> assertExtracted)
            where TResult : IResult<Guid, string>
        {
            var task = Task.FromResult(result);

            await assertExtracted(task).ConfigureAwait(false);
        }
    }
}
