using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrElse
{
    [TestFixture]
    internal class Plain_Task_Should
    {
        private const string ExpectedFault = "fallback";

        private static TestCaseData CreateConvertCase<TResult>(
            TResult source,
            string expectedResult,
            Func<Task<TResult>, Task<string>> method)
            where TResult : IResult<string>
        {
            return new(source, method)
            {
                ExpectedResult = expectedResult,
            };
        }

        private static readonly Func<Task<Result<string>>, Task<string>>[] ResultMethods =
        {
            result => result.GetFaultOrElse(() => ExpectedFault),
            result => result.GetFaultOrElse(ExpectedFault),
        };

        private static readonly Func<Task<IResult<string>>, Task<string>>[] InterfaceMethods =
        {
            result => result.GetFaultOrElse(() => ExpectedFault),
            result => result.GetFaultOrElse(ExpectedFault),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TResult>(
            TResult success,
            TResult failure,
            string faultValue,
            IEnumerable<Func<Task<TResult>, Task<string>>> methods)
            where TResult : IResult<string>
        {
            return from testCase in new[]
                {
                    (Source: success, Result: ExpectedFault),
                    (Source: failure, Result: faultValue),
                }
                from method in methods
                select CreateConvertCase(testCase.Source, testCase.Result, method);
        }

        private static readonly IEnumerable<TestCaseData> ConvertCases =
            GenerateCases(Result<string>.Succeed(), Result<string>.Fail("bar"), "bar", ResultMethods)
                .Concat(GenerateCases(Result<string>.Succeed(), Result<string>.Fail("bar"), "bar", InterfaceMethods));

        [TestCaseSource(nameof(ConvertCases))]
        public Task<string> Return_Result<TResult>(TResult result, Func<Task<TResult>, Task<string>> callGetOrElse)
            where TResult : IResult<string>
        {
            var task = Task.FromResult(result);

            return callGetOrElse(task);
        }

        private static string AssertIsNotCalled()
        {
            Assert.Fail("Backup fault factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<string>>, Task<string>>[] ResultAssertNotCalledMethods =
        {
            result => result.GetFaultOrElse(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<string>>, Task<string>>[] InterfaceAssertNotCalledMethods =
        {
            result => result.GetFaultOrElse(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<string>>> methods)
            where TResult : IResult<string>
        {
            return methods.Select(method => new TestCaseData(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfFaultCases =
            CreateDoNotCallFactoryCases(Result<string>.Fail("fault"), ResultAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Result<string>.Fail("fault"), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfFaultCases))]
        public async Task Do_Not_Call_Delegate_If_Fault<TResult>(TResult result, Func<Task<TResult>, Task<string>> assertExtracted)
            where TResult : IResult<string>
        {
            var task = Task.FromResult(result);

            await assertExtracted(task).ConfigureAwait(false);
        }
    }
}
