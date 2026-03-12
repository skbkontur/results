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
    internal class Fault_Task_Should
    {
        private const string ExpectedFault = "fallback";

        private static TestCaseData CreateConvertCase<TResult>(
            TResult source,
            string expectedResult,
            Func<Task<TResult>, Task<string>> method)
            where TResult : IResult<string, Guid>
        {
            return new(source, method)
            {
                ExpectedResult = expectedResult,
            };
        }

        private static readonly Func<Task<Result<string, Guid>>, Task<string>>[] ResultMethods =
        {
            result => result.GetFaultOrElse(_ => ExpectedFault),
            result => result.GetFaultOrElse(() => ExpectedFault),
            result => result.GetFaultOrElse(ExpectedFault),
        };

        private static readonly Func<Task<IResult<string, Guid>>, Task<string>>[] InterfaceMethods =
        {
            result => result.GetFaultOrElse(_ => ExpectedFault),
            result => result.GetFaultOrElse(() => ExpectedFault),
            result => result.GetFaultOrElse(ExpectedFault),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TResult>(
            Func<Guid, TResult> successFactory,
            TResult failure,
            string faultValue,
            IEnumerable<Func<Task<TResult>, Task<string>>> methods)
            where TResult : IResult<string, Guid>
        {
            return from testCase in new[]
                {
                    (Source: successFactory(Guid.NewGuid()), Result: ExpectedFault),
                    (Source: failure, Result: faultValue),
                }
                from method in methods
                select CreateConvertCase(testCase.Source, testCase.Result, method);
        }

        private static readonly IEnumerable<TestCaseData> ConvertCases =
            GenerateCases(Result<string, Guid>.Succeed, Result<string, Guid>.Fail("bar"), "bar", ResultMethods)
                .Concat(GenerateCases(Result<string, Guid>.Succeed, Result<string, Guid>.Fail("bar"), "bar", InterfaceMethods));

        [TestCaseSource(nameof(ConvertCases))]
        public Task<string> Return_Result<TResult>(TResult result, Func<Task<TResult>, Task<string>> callGetOrElse)
            where TResult : IResult<string, Guid>
        {
            var task = Task.FromResult(result);

            return callGetOrElse(task);
        }

        private static TestCaseData CreateUseValueCase<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<string>> callGetOrElse)
            where TResult : IResult<string, int>
        {
            return new(factory, callGetOrElse);
        }

        private static readonly TestCaseData[] UseValueCases =
        {
            CreateUseValueCase(Result<string, int>.Succeed, (result, map) => result.GetFaultOrElse(map)),
            CreateUseValueCase<IResult<string, int>>(Result<string, int>.Succeed, (result, map) => result.GetFaultOrElse(map)),
        };

        [TestCaseSource(nameof(UseValueCases))]
        public async Task Pass_Value_To_Factory<TResult>(
            Func<int, TResult> factory,
            Func<Task<TResult>, Func<int, string>, Task<string>> callGetOrElse)
            where TResult : IResult<string, int>
        {
            var source = Task.FromResult(factory(777));

            var fault = await callGetOrElse(source, i => i.ToString(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);

            fault.Should().Be("777");
        }

        private static string AssertIsNotCalled()
        {
            Assert.Fail("Backup fault factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Result<string, Guid>>, Task<string>>[] ResultAssertNotCalledMethods =
        {
            result => result.GetFaultOrElse(_ => AssertIsNotCalled()),
            result => result.GetFaultOrElse(AssertIsNotCalled),
        };

        private static readonly Func<Task<IResult<string, Guid>>, Task<string>>[] InterfaceAssertNotCalledMethods =
        {
            result => result.GetFaultOrElse(_ => AssertIsNotCalled()),
            result => result.GetFaultOrElse(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TResult>(
            TResult failure,
            IEnumerable<Func<Task<TResult>, Task<string>>> methods)
            where TResult : IResult<string, Guid>
        {
            return methods.Select(method => new TestCaseData(failure, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfFailureCases =
            CreateDoNotCallFactoryCases(Result<string, Guid>.Fail("fault"), ResultAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Result<string, Guid>.Fail("fault"), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfFailureCases))]
        public async Task Do_Not_Call_Delegate_If_Failure<TResult>(TResult result, Func<Task<TResult>, Task<string>> assertExtracted)
            where TResult : IResult<string, Guid>
        {
            var task = Task.FromResult(result);

            await assertExtracted(task).ConfigureAwait(false);
        }
    }
}
