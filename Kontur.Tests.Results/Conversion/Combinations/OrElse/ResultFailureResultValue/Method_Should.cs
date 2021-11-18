using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.OrElse.ResultFailureResultValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(ResultFailure<int> Result1, Result<string, Guid> Result2, Result<string, Guid> Result)> CreateCases()
        {
            const string fault = "bar";
            yield return (Result.Fail(14), Result<string, Guid>.Fail(fault), Result<string, Guid>.Fail(fault));

            var succeed = Result<string, Guid>.Succeed(Guid.NewGuid());
            yield return (Result.Fail(123), succeed, succeed);
        }

        private static readonly Func<ResultFailure<int>, Func<int, Result<string, Guid>>, ValueTask<Result<string, Guid>>>[] PassMethodsValueTasks =
        {
            (result, factory) => ValueTaskFactory.Create(result).OrElse(factory),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(async fault => factory(fault)),
        };

        private static readonly Func<ResultFailure<int>, Func<int, Result<string, Guid>>, Task<Result<string, Guid>>>[] PassMethodsTasks =
        {
            (result, factory) => Task.FromResult(result).OrElse(factory),
            (result, factory) => Task.FromResult(result).OrElse(fault => Task.FromResult(factory(fault))),
            (result, factory) => Task.FromResult(result).OrElse(fault => ValueTaskFactory.Create(factory(fault))),
            (result, factory) => Task.FromResult(result).OrElse(async fault => factory(fault)),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(fault => Task.FromResult(factory(fault))),
        };

        private static IEnumerable<Func<ResultFailure<int>, Func<int, Result<string, Guid>>, Task<Result<string, Guid>>>> PassMethods()
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

        private static readonly Func<ResultFailure<int>, Func<Result<string, Guid>>, ValueTask<Result<string, Guid>>>[] FactoryMethodsValueTasks =
        {
            (result, factory) => ValueTaskFactory.Create(result).OrElse(factory),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).OrElse(async () => factory()),
        };

        private static readonly Func<ResultFailure<int>, Func<Result<string, Guid>>, Task<Result<string, Guid>>>[] FactoryMethodsTasks =
        {
            (result, factory) => Task.FromResult(result).OrElse(factory),
            (result, factory) => Task.FromResult(result).OrElse(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).OrElse(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).OrElse(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).OrElse(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<ResultFailure<int>, Func<Result<string, Guid>>, Task<Result<string, Guid>>>> FactoryMethods()
        {
            yield return (result, factory) => Task.FromResult(result.OrElse(factory));

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

        private static readonly Func<ResultFailure<int>, Result<string, Guid>, ValueTask<Result<string, Guid>>>[] PlainMethodsValueTasks =
        {
            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(result2),
            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<ResultFailure<int>, Result<string, Guid>, Task<Result<string, Guid>>>[] PlainMethodsTasks =
        {
            (result1, result2) => Task.FromResult(result1).OrElse(result2),
            (result1, result2) => Task.FromResult(result1).OrElse(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).OrElse(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).OrElse(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<ResultFailure<int>, Result<string, Guid>, Task<Result<string, Guid>>>> AllMethods()
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
            Func<Task<Result<string, Guid>>> then,
            Result<string, Guid> expectedResult)
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
        public async Task<Result<string, Guid>> Process(Func<Task<Result<string, Guid>>> orElse)
        {
            return await orElse().ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> PassFaultCases =
            PassMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(PassFaultCases))]
        public async Task Pass_Fault_If_Failure(Func<ResultFailure<int>, Func<int, Result<string, Guid>>, Task<Result<string, Guid>>> orElse)
        {
            const int expected = 123;
            var source = Result.Fail(expected);

            var result = await orElse(
                source,
                fault => Result<string, Guid>.Fail(fault.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);

            result.Should().Be(Result<string, Guid>.Fail(expected.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
