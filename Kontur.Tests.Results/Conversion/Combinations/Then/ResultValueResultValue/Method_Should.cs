using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.Then.ResultValueResultValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(Result<Guid, int> Result1, Result<Guid, string> Result2, Result<Guid, string> Result)> CreateCases()
        {
            var example1 = Result<Guid, string>.Succeed("bar");
            yield return (Result<Guid, int>.Succeed(15), example1, example1);

            var fault1 = Guid.NewGuid();
            yield return (Result<Guid, int>.Fail(fault1), Result<Guid, string>.Succeed("unused"), Result<Guid, string>.Fail(fault1));

            var example2 = Result<Guid, string>.Fail(Guid.NewGuid());
            yield return (Result<Guid, int>.Succeed(16), example2, example2);

            var fault2 = Guid.NewGuid();
            yield return (Result<Guid, int>.Fail(fault2), Result<Guid, string>.Fail(Guid.NewGuid()), Result<Guid, string>.Fail(fault2));
        }

        private static readonly Func<Result<Guid, int>, Func<int, Result<Guid, string>>, ValueTask<Result<Guid, string>>>[] PassMethodsValueTasks =
        {
            (result, factory) => result.Then(value => ValueTaskFactory.Create(factory(value))),
            (result, factory) => result.Then(async value => factory(value)),

            (result, factory) => ValueTaskFactory.Create(result).Then(factory),
            (result, factory) => ValueTaskFactory.Create(result).Then(value => ValueTaskFactory.Create(factory(value))),
            (result, factory) => ValueTaskFactory.Create(result).Then(async value => factory(value)),
        };

        private static readonly Func<Result<Guid, int>, Func<int, Result<Guid, string>>, Task<Result<Guid, string>>>[] PassMethodsTasks =
        {
            (result, factory) => result.Then(value => Task.FromResult(factory(value))),

            (result, factory) => Task.FromResult(result).Then(factory),
            (result, factory) => Task.FromResult(result).Then(value => Task.FromResult(factory(value))),
            (result, factory) => Task.FromResult(result).Then(value => ValueTaskFactory.Create(factory(value))),
            (result, factory) => Task.FromResult(result).Then(async value => factory(value)),

            (result, factory) => ValueTaskFactory.Create(result).Then(value => Task.FromResult(factory(value))),
        };

        private static IEnumerable<Func<Result<Guid, int>, Func<int, Result<Guid, string>>, Task<Result<Guid, string>>>> PassMethods()
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

        private static readonly Func<Result<Guid, int>, Func<Result<Guid, string>>, ValueTask<Result<Guid, string>>>[] FactoryMethodsValueTasks =
        {
            (result, factory) => result.Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => result.Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(factory),
            (result, factory) => ValueTaskFactory.Create(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => ValueTaskFactory.Create(result).Then(async () => factory()),
        };

        private static readonly Func<Result<Guid, int>, Func<Result<Guid, string>>, Task<Result<Guid, string>>>[] FactoryMethodsTasks =
        {
            (result, factory) => result.Then(() => Task.FromResult(factory())),

            (result, factory) => Task.FromResult(result).Then(factory),
            (result, factory) => Task.FromResult(result).Then(() => Task.FromResult(factory())),
            (result, factory) => Task.FromResult(result).Then(() => ValueTaskFactory.Create(factory())),
            (result, factory) => Task.FromResult(result).Then(async () => factory()),

            (result, factory) => ValueTaskFactory.Create(result).Then(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<Result<Guid, int>, Func<Result<Guid, string>>, Task<Result<Guid, string>>>> FactoryMethods()
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

        private static readonly Func<Result<Guid, int>, Result<Guid, string>, ValueTask<Result<Guid, string>>>[] PlainMethodsValueTasks =
        {
            (result1, result2) => result1.Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(result2),
            (result1, result2) => ValueTaskFactory.Create(result1).Then(ValueTaskFactory.Create(result2)),
        };

        private static readonly Func<Result<Guid, int>, Result<Guid, string>, Task<Result<Guid, string>>>[] PlainMethodsTasks =
        {
            (result1, result2) => result1.Then(Task.FromResult(result2)),

            (result1, result2) => Task.FromResult(result1).Then(result2),
            (result1, result2) => Task.FromResult(result1).Then(Task.FromResult(result2)),
            (result1, result2) => Task.FromResult(result1).Then(ValueTaskFactory.Create(result2)),

            (result1, result2) => ValueTaskFactory.Create(result1).Then(Task.FromResult(result2)),
        };

        private static IEnumerable<Func<Result<Guid, int>, Result<Guid, string>, Task<Result<Guid, string>>>> PlainMethods()
        {
            yield return async (result1, result2) => result1.Then(result2);

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

        private static readonly IEnumerable<TestCaseData> Cases =
            from testCase in CreateCases()
            from method in PlainMethods()
            select new TestCaseData(testCase.Result1, testCase.Result2, method).Returns(testCase.Result);

        [TestCaseSource(nameof(Cases))]
        public Task<Result<Guid, string>> Process(
            Result<Guid, int> result1,
            Result<Guid, string> result2,
            Func<Result<Guid, int>, Result<Guid, string>, Task<Result<Guid, string>>> then)
        {
            return then(result1, result2);
        }

        private static readonly IEnumerable<TestCaseData> PassValueCases =
            PassMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(PassValueCases))]
        public async Task Pass_Value_If_Success(Func<Result<Guid, int>, Func<int, Result<Guid, string>>, Task<Result<Guid, string>>> then)
        {
            var source = Result<Guid, int>.Succeed(123);

            var result = await then(
                source,
                value => Result<Guid, string>.Succeed(value.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);

            result.Should().Be(Result<Guid, string>.Succeed("123"));
        }

        private static Result<Guid, string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases =
            FactoryMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_Failure(Func<Result<Guid, int>, Func<Result<Guid, string>>, Task<Result<Guid, string>>> then)
        {
            var result = Result<Guid, int>.Fail(Guid.NewGuid());

            await then(result, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
