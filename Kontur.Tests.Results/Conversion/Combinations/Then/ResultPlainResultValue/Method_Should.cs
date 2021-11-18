using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.Then.ResultPlainResultValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(Result<int> Result1, Result<int, string> Result2, Result<int, string> Result)> CreateCases()
        {
            var example1 = Result<int, string>.Succeed("bar");
            yield return (Result<int>.Succeed(), example1, example1);

            const int fault1 = 20;
            yield return (Result<int>.Fail(fault1), Result<int, string>.Succeed("unused"), Result<int, string>.Fail(fault1));

            var example2 = Result<int, string>.Fail(1);
            yield return (Result<int>.Succeed(), example2, example2);

            const int fault2 = 15;
            yield return (Result<int>.Fail(fault2), Result<int, string>.Fail(2), Result<int, string>.Fail(fault2));
        }

        private static readonly Func<Result<int>, Func<Result<int, string>>, ValueTask<Result<int, string>>>[] FactoryMethodsValueTasks =
        {
            (result, factory) => result.Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => result.Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(factory),
            (result, factory) => ValueTaskFactory.Create(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).Then(async () => factory()),
        };

        private static readonly Func<Result<int>, Func<Result<int, string>>, Task<Result<int, string>>>[] FactoryMethodsTasks =
        {
            (result, factory) => result.Then(() => Task.FromResult(factory())),

            (result, factory) => Task.FromResult(result).Then(factory),
            (result, factory) => Task.FromResult(result).Then(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<Result<int>, Func<Result<int, string>>, Task<Result<int, string>>>> FactoryMethods()
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

        private static readonly Func<Result<int>, Result<int, string>, ValueTask<Result<int, string>>>[] PlainMethodsValueTasks =
        {
            (result1, result2) => result1.Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(result2),
            (result1, result2) => ValueTaskFactory.Create(result1).Then(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<Result<int>, Result<int, string>, Task<Result<int, string>>>[] PlainMethodsTasks =
        {
            (result1, result2) => result1.Then(Task.FromResult(result2)),

            (result1, result2) => Task.FromResult(result1).Then(result2),
            (result1, result2) => Task.FromResult(result1).Then(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<Result<int>, Result<int, string>, Task<Result<int, string>>>> AllMethods()
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
            Func<Task<Result<int, string>>> then,
            Result<int, string> expectedResult)
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
        public async Task<Result<int, string>> Process(Func<Task<Result<int, string>>> then)
        {
            return await then().ConfigureAwait(false);
        }

        private static Result<int, string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases =
            FactoryMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_Failure(Func<Result<int>, Func<Result<int, string>>, Task<Result<int, string>>> then)
        {
            var result = Result<int>.Fail(0);

            await then(result, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
