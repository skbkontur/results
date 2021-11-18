using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.OrElse.ResultFailureResultFailure
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(ResultFailure<int> Result1, ResultFailure<string> Result2, ResultFailure<string> Result)> CreateResultFailureCases()
        {
            const string fault = "fault";
            var result = Result.Fail(fault);
            yield return (Result.Fail(1), result, result);
        }

        private static readonly Func<ResultFailure<int>, Func<int, ResultFailure<string>>, ValueTask<ResultFailure<string>>>[] PassResultFailureMethodsValueTasks =
        {
            (result, factory) => ValueTaskFactory.Create(result).OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(async fault => factory(fault)),
        };

        private static readonly Func<ResultFailure<int>, Func<int, ResultFailure<string>>, Task<ResultFailure<string>>>[] PassResultFailureMethodsTasks =
        {
            (result, factory) => Task.FromResult(result).OrElse(fault => Task.FromResult(factory(fault))),
            (result, factory) => Task.FromResult(result).OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => Task.FromResult(result).OrElse(async fault => factory(fault)),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(fault => Task.FromResult(factory(fault))),
        };

        private static IEnumerable<Func<ResultFailure<int>, Func<int, ResultFailure<string>>, Task<ResultFailure<string>>>> PassResultFailureMethods()
        {
            yield return (result, factory) => Task.FromResult(result.OrElse(factory));

            foreach (var method in PassResultFailureMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PassResultFailureMethodsValueTasks)
            {
                yield return async (result, factory) => await method(result, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<ResultFailure<int>, Func<ResultFailure<string>>, ValueTask<ResultFailure<string>>>[] FactoryResultFailureMethodsValueTasks =
        {
            (result, factory) => ValueTaskFactory.Create(result).OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(async () => factory()),
        };

        private static readonly Func<ResultFailure<int>, Func<ResultFailure<string>>, Task<ResultFailure<string>>>[] FactoryResultFailureMethodsTasks =
        {
            (result, factory) => Task.FromResult(result).OrElse(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).OrElse(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<ResultFailure<int>, Func<ResultFailure<string>>, Task<ResultFailure<string>>>> FactoryResultFailureMethods()
        {
            yield return (result1, result2) => Task.FromResult(result1.OrElse(result2));

            foreach (var method in PassResultFailureMethods())
            {
                yield return (result, factory) => method(result, _ => factory());
            }

            foreach (var method in FactoryResultFailureMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in FactoryResultFailureMethodsValueTasks)
            {
                yield return async (result, factory) => await method(result, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<ResultFailure<int>, ResultFailure<string>, ValueTask<ResultFailure<string>>>[] PlainResultFailureMethodsValueTasks =
        {
            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<ResultFailure<int>, ResultFailure<string>, Task<ResultFailure<string>>>[] PlainResultFailureMethodsTasks =
        {
            (result1, result2) => Task.FromResult(result1).OrElse(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).OrElse(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<ResultFailure<int>, ResultFailure<string>, Task<ResultFailure<string>>>> AllResultFailureMethods()
        {
            yield return (result1, result2) => Task.FromResult(result1.OrElse(result2));

            foreach (var method in FactoryResultFailureMethods())
            {
                yield return (result1, result2) => method(result1, () => result2);
            }

            foreach (var method in PlainResultFailureMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PlainResultFailureMethodsValueTasks)
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
            from testCase in CreateResultFailureCases()
            from method in AllResultFailureMethods()
            select CreateProcessCase(() => method(testCase.Result1, testCase.Result2), testCase.Result);

        [TestCaseSource(nameof(Cases))]
        public async Task<ResultFailure<string>> Process(Func<Task<ResultFailure<string>>> orElse)
        {
            return await orElse().ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> PassFaultCases =
            PassResultFailureMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(PassFaultCases))]
        public async Task Pass_Fault_If_Failure(Func<ResultFailure<int>, Func<int, ResultFailure<string>>, Task<ResultFailure<string>>> orElse)
        {
            const int expectedFault = 123;
            var source = Result.Fail(expectedFault);

            var result = await orElse(
                source,
                fault => Result.Fail(fault.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);

            result.Should().Be(Result.Fail(expectedFault.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
