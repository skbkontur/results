using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.OrElse.ResultPlainResultFailure
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(Result<int> Result1, ResultFailure<string> Result2, Result<string> Result)> CreateCases()
        {
            yield return (Result<int>.Succeed(), Result.Fail("unused"), Result<string>.Succeed());

            const string fault = "fault";
            yield return (Result<int>.Fail(1), Result.Fail(fault), Result<string>.Fail(fault));
        }

        private static readonly Func<Result<int>, Func<int, ResultFailure<string>>, ValueTask<Result<string>>>[] PassMethodsValueTasks =
        {
            (result, factory) => result.OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => result.OrElse(async fault => factory(fault)),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(async fault => factory(fault)),
        };

        private static readonly Func<Result<int>, Func<int, ResultFailure<string>>, Task<Result<string>>>[] PassMethodsTasks =
        {
            (result, factory) => result.OrElse(fault => Task.FromResult(factory(fault))),

            (result, factory) => Task.FromResult(result).OrElse(fault => Task.FromResult(factory(fault))),
            (result, factory) => Task.FromResult(result).OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => Task.FromResult(result).OrElse(async fault => factory(fault)),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(fault => Task.FromResult(factory(fault))),
        };

        private static IEnumerable<Func<Result<int>, Func<int, ResultFailure<string>>, Task<Result<string>>>> PassMethods()
        {
            yield return (result, factory) => Task.FromResult(result.OrElse(factory));

            foreach (var method in PassMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PassMethodsValueTasks)
            {
                yield return async (result, factory) => await method(result, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<Result<int>, Func<ResultFailure<string>>, ValueTask<Result<string>>>[] FactoryMethodsValueTasks =
        {
            (result, factory) => result.OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => result.OrElse(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(async () => factory()),
        };

        private static readonly Func<Result<int>, Func<ResultFailure<string>>, Task<Result<string>>>[] FactoryMethodsTasks =
        {
            (result, factory) => result.OrElse(() => Task.FromResult(factory())),

            (result, factory) => Task.FromResult(result).OrElse(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).OrElse(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<Result<int>, Func<ResultFailure<string>>, Task<Result<string>>>> FactoryMethods()
        {
            yield return (result1, result2) => Task.FromResult(result1.OrElse(result2));

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

        private static readonly Func<Result<int>, ResultFailure<string>, ValueTask<Result<string>>>[] PlainMethodsValueTasks =
        {
            (result1, result2) => result1.OrElse(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<Result<int>, ResultFailure<string>, Task<Result<string>>>[] PlainMethodsTasks =
        {
            (result1, result2) => result1.OrElse(Task.FromResult(result2)),

            (result1, result2) => Task.FromResult(result1).OrElse(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).OrElse(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<Result<int>, ResultFailure<string>, Task<Result<string>>>> AllMethods()
        {
            yield return (result1, result2) => Task.FromResult(result1.OrElse(result2));

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
            Func<Task<Result<string>>> then,
            Result<string> expectedResult)
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
        public async Task<Result<string>> Process(Func<Task<Result<string>>> orElse)
        {
            return await orElse().ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> PassFaultCases =
            PassMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(PassFaultCases))]
        public async Task Pass_Fault_If_Failure(Func<Result<int>, Func<int, ResultFailure<string>>, Task<Result<string>>> orElse)
        {
            const int expectedFault = 123;
            var source = Result<int>.Fail(expectedFault);

            var result = await orElse(
                source,
                fault => Result.Fail(fault.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);

            result.Should().Be(Result<string>.Fail(expectedFault.ToString(CultureInfo.InvariantCulture)));
        }

        private static ResultFailure<string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on Success");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases =
            FactoryMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_Success(Func<Result<int>, Func<ResultFailure<string>>, Task<Result<string>>> orElse)
        {
            var result = Result<int>.Succeed();

            await orElse(result, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
