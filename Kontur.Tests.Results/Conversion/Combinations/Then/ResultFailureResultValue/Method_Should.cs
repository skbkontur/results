using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.Then.ResultFailureResultValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(ResultFailure<int> Result1, Result<int, string> Result2, ResultFailure<int> Result)> CreateCases()
        {
            var fault1 = Result.Fail(20);
            yield return (fault1, Result<int, string>.Succeed("unused"), fault1);

            var fault2 = Result.Fail(15);
            yield return (fault2, Result<int, string>.Fail(2), fault2);
        }

        private static readonly Func<ResultFailure<int>, Func<Result<int, string>>, ValueTask<ResultFailure<int>>>[] FactoryMethodsValueTasks =
        {
            (result, factory) => ValueTaskFactory.Create(result).Then(factory),
            (result, factory) => ValueTaskFactory.Create(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).Then(async () => factory()),
        };

        private static readonly Func<ResultFailure<int>, Func<Result<int, string>>, Task<ResultFailure<int>>>[] FactoryMethodsTasks =
        {
            (result, factory) => Task.FromResult(result).Then(factory),
            (result, factory) => Task.FromResult(result).Then(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<ResultFailure<int>, Func<Result<int, string>>, Task<ResultFailure<int>>>> FactoryMethods()
        {
            yield return (result, factory) => Task.FromResult(result.Then(factory));

            foreach (var method in FactoryMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in FactoryMethodsValueTasks)
            {
                yield return async (result, factory) => await method(result, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<ResultFailure<int>, Result<int, string>, ValueTask<ResultFailure<int>>>[] PlainMethodsValueTasks =
        {
            (result1, result2) => ValueTaskFactory.Create(result1).Then(result2),
            (result1, result2) => ValueTaskFactory.Create(result1).Then(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<ResultFailure<int>, Result<int, string>, Task<ResultFailure<int>>>[] PlainMethodsTasks =
        {
            (result1, result2) => Task.FromResult(result1).Then(result2),
            (result1, result2) => Task.FromResult(result1).Then(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<ResultFailure<int>, Result<int, string>, Task<ResultFailure<int>>>> AllMethods()
        {
            yield return (result1, result2) => Task.FromResult(result1.Then(result2));

            foreach (var method in FactoryMethods())
            {
                yield return (result1, result2) => method(result1, () => result2);
            }

            foreach (var method in PlainMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PlainMethodsValueTasks)
            {
                yield return async (result1, result2) => await method(result1, result2).ConfigureAwait(false);
            }
        }

        private static TestCaseData CreateProcessCase(
            Func<Task<ResultFailure<int>>> then,
            ResultFailure<int> expectedResult)
        {
            return new(then)
            {
                ExpectedResult = expectedResult,
            };
        }

        private static readonly IEnumerable<TestCaseData> Cases =
            from testCase in CreateCases()
            from method in AllMethods()
            select CreateProcessCase(() => method(testCase.Result1, testCase.Result2), testCase.Result);

        [TestCaseSource(nameof(Cases))]
        public async Task<ResultFailure<int>> Process(Func<Task<ResultFailure<int>>> then)
        {
            return await then().ConfigureAwait(false);
        }

        private static Result<int, string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases = FactoryMethods()
                    .Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_Failure(Func<ResultFailure<int>, Func<Result<int, string>>, Task<ResultFailure<int>>> then)
        {
            var result = Result.Fail(0);

            await then(result, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
