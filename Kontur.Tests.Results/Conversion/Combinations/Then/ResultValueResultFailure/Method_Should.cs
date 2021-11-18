using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.Then.ResultValueResultFailure
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(Result<string, int> Result1, ResultFailure<string> Result2, ResultFailure<string> Result)> CreateCases()
        {
            var resultFailure = Result.Fail("fault");
            yield return (Result<string, int>.Succeed(15), resultFailure, resultFailure);

            const string fault = "bar";
            yield return (Result<string, int>.Fail(fault), Result.Fail("unused"), Result.Fail(fault));
        }

        private static readonly Func<Result<string, int>, Func<int, ResultFailure<string>>, ValueTask<ResultFailure<string>>>[] PassMethodsValueTasks =
        {
            (result, factory) => result.Then(value => ValueTaskFactory.Create(factory(value))),
            (result, factory) => result.Then(async value => factory(value)),

            (result, factory) => ValueTaskFactory.Create(result).Then(value => ValueTaskFactory.Create(factory(value))),
            (result, factory) => ValueTaskFactory.Create(result).Then(async value => factory(value)),
        };

        private static readonly Func<Result<string, int>, Func<int, ResultFailure<string>>, Task<ResultFailure<string>>>[] PassMethodsTasks =
        {
            (result, factory) => result.Then(value => Task.FromResult(factory(value))),

            (result, factory) => Task.FromResult(result).Then(value => Task.FromResult(factory(value))),
            (result, factory) => Task.FromResult(result).Then(value => ValueTaskFactory.Create(factory(value))),
            (result, factory) => Task.FromResult(result).Then(async value => factory(value)),

            (result, factory) => ValueTaskFactory.Create(result).Then(value => Task.FromResult(factory(value))),
        };

        private static IEnumerable<Func<Result<string, int>, Func<int, ResultFailure<string>>, Task<ResultFailure<string>>>> PassMethods()
        {
            yield return (result, factory) => Task.FromResult(result.Then(factory));

            foreach (var method in PassMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PassMethodsValueTasks)
            {
                yield return async (result, factory) => await method(result, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<Result<string, int>, Func<ResultFailure<string>>, ValueTask<ResultFailure<string>>>[] FactoryMethodsValueTasks =
        {
            (result, factory) => result.Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => result.Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).Then(async () => factory()),
        };

        private static readonly Func<Result<string, int>, Func<ResultFailure<string>>, Task<ResultFailure<string>>>[] FactoryMethodsTasks =
        {
            (result, factory) => result.Then(() => Task.FromResult(factory())),

            (result, factory) => Task.FromResult(result).Then(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<Result<string, int>, Func<ResultFailure<string>>, Task<ResultFailure<string>>>> FactoryMethods()
        {
            yield return (result, factory) => Task.FromResult(result.Then(factory));

            foreach (var method in PassMethods())
            {
                yield return (result, factory) => method(result, _ => factory());
            }

            foreach (var method in FactoryMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in FactoryMethodsValueTasks)
            {
                yield return async (result, factory) => await method(result, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<Result<string, int>, ResultFailure<string>, ValueTask<ResultFailure<string>>>[] PlainMethodsValueTasks =
        {
            (result1, result2) => result1.Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<Result<string, int>, ResultFailure<string>, Task<ResultFailure<string>>>[] PlainMethodsTasks =
        {
            (result1, result2) => result1.Then(Task.FromResult(result2)),

            (result1, result2) => Task.FromResult(result1).Then(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<Result<string, int>, ResultFailure<string>, Task<ResultFailure<string>>>> AllMethods()
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
            Func<Task<ResultFailure<string>>> then,
            ResultFailure<string> expectedResult)
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
        public async Task<ResultFailure<string>> Process(Func<Task<ResultFailure<string>>> then)
        {
            return await then().ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> PassValueCases =
            PassMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(PassValueCases))]
        public async Task Pass_Value_If_Success(Func<Result<string, int>, Func<int, ResultFailure<string>>, Task<ResultFailure<string>>> then)
        {
            const int expected = 777;
            var source = Result<string, int>.Succeed(expected);

            var result = await then(
                source,
                value => Result.Fail(value.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);

            result.Should().Be(Result.Fail(expected.ToString(CultureInfo.InvariantCulture)));
        }

        private static ResultFailure<string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases =
            FactoryMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_Failure(Func<Result<string, int>, Func<ResultFailure<string>>, Task<ResultFailure<string>>> then)
        {
            var result = Result<string, int>.Fail("fault");

            await then(result, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
