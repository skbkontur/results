using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;
#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.OrElse.OptionalOptional
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(Optional<string> Optional1, Optional<string> Optional2, Optional<string> Result)> CreateCases()
        {
            var none = Optional<string>.None();
            var some = Optional<string>.Some("example");

            yield return (none, none, none);
            yield return (some, none, some);
            yield return (none, some, some);
            yield return (some, Optional<string>.Some("unused"), some);
        }

        private static readonly Func<Optional<string>, Func<Optional<string>>, ValueTask<Optional<string>>>[] FactoryMethodsValueTasks =
        {
            (optional, factory) => optional.OrElse(() => ValueTaskFactory.Create(factory())),
            (optional, factory) => optional.OrElse(async () => factory()),

            (optional, factory) => ValueTaskFactory.Create(optional).OrElse(factory),
            (optional, factory) => ValueTaskFactory.Create(optional).OrElse(() => ValueTaskFactory.Create(factory())),
            (optional, factory) => ValueTaskFactory.Create(optional).OrElse(async () => factory()),
        };

        private static readonly Func<Optional<string>, Func<Optional<string>>, Task<Optional<string>>>[] FactoryMethodsTasks =
        {
            (optional, factory) => optional.OrElse(() => Task.FromResult(factory())),

            (optional, factory) => Task.FromResult(optional).OrElse(factory),
            (optional, factory) => Task.FromResult(optional).OrElse(() => Task.FromResult(factory())),
            (optional, factory) => Task.FromResult(optional).OrElse(() => ValueTaskFactory.Create(factory())),
            (optional, factory) => Task.FromResult(optional).OrElse(async () => factory()),

            (optional, factory) => ValueTaskFactory.Create(optional).OrElse(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<Optional<string>, Func<Optional<string>>, Task<Optional<string>>>> FactoryMethods()
        {
            yield return (optional, factory) => Task.FromResult(optional.OrElse(factory));

            foreach (var method in FactoryMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in FactoryMethodsValueTasks)
            {
                yield return async (optional, factory) => await method(optional, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<Optional<string>, Optional<string>, ValueTask<Optional<string>>>[] PlainMethodsValueTasks =
        {
            (optional1, optional2) => optional1.OrElse(ValueTaskFactory.Create(optional2)),

            (optional1, optional2) => ValueTaskFactory.Create(optional1).OrElse(optional2),
            (optional1, optional2) => ValueTaskFactory.Create(optional1).OrElse(ValueTaskFactory.Create(optional2)),
        };

        private static readonly Func<Optional<string>, Optional<string>, Task<Optional<string>>>[] PlainMethodsTasks =
        {
            (optional1, optional2) => optional1.OrElse(Task.FromResult(optional2)),

            (optional1, optional2) => Task.FromResult(optional1).OrElse(optional2),
            (optional1, optional2) => Task.FromResult(optional1).OrElse(Task.FromResult(optional2)),
            (optional1, optional2) => Task.FromResult(optional1).OrElse(ValueTaskFactory.Create(optional2)),

            (optional1, optional2) => ValueTaskFactory.Create(optional1).OrElse(Task.FromResult(optional2)),
        };

        private static IEnumerable<Func<Optional<string>, Optional<string>, Task<Optional<string>>>> AllMethods()
        {
            yield return (optional1, optional2) => Task.FromResult(optional1.OrElse(optional2));

            foreach (var method in FactoryMethods())
            {
                yield return (optional1, optional2) => method(optional1, () => optional2);
            }

            foreach (var method in PlainMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PlainMethodsValueTasks)
            {
                yield return async (optional1, optional2) => await method(optional1, optional2).ConfigureAwait(false);
            }
        }

        private static readonly IEnumerable<TestCaseData> Cases =
            from testCase in CreateCases()
            from method in AllMethods()
            select new TestCaseData(testCase.Optional1, testCase.Optional2, method).Returns(testCase.Result);

        [TestCaseSource(nameof(Cases))]
        public async Task<Optional<string>> Process(
            Optional<string> optional1,
            Optional<string> optional2,
            Func<Optional<string>, Optional<string>, Task<Optional<string>>> orElse)
        {
            return await orElse(optional1, optional2).ConfigureAwait(false);
        }

        private static Optional<string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on Some");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases =
            FactoryMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_Some(Func<Optional<string>, Func<Optional<string>>, Task<Optional<string>>> orElse)
        {
            var optional = Optional<string>.Some("value");

            await orElse(optional, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
