using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;
#pragma warning disable 1998

namespace Kontur.Tests.Results.Conversion.Combinations.Then.OptionalOptional
{
    [TestFixture]
    internal class Method_Should
    {
        private static IEnumerable<(Optional<int> Option1, Optional<string> Option2, Optional<string> Result)> CreateCases()
        {
            yield return (Optional<int>.None(), Optional<string>.None(), Optional<string>.None());
            yield return (Optional<int>.Some(1), Optional<string>.None(), Optional<string>.None());
            yield return (Optional<int>.None(), Optional<string>.Some("unused"), Optional<string>.None());

            var some = Optional<string>.Some("value");
            yield return (Optional<int>.Some(1), some, some);
        }

        private static readonly Func<Optional<int>, Func<int, Optional<string>>, ValueTask<Optional<string>>>[] PassMethodsValueTasks =
        {
            (optional, factory) => optional.Then(value => ValueTaskFactory.Create(factory(value))),
            (optional, factory) => optional.Then(async value => factory(value)),

            (optional, factory) => ValueTaskFactory.Create(optional).Then(factory),
            (optional, factory) => ValueTaskFactory.Create(optional).Then(value => ValueTaskFactory.Create(factory(value))),
            (optional, factory) => ValueTaskFactory.Create(optional).Then(async value => factory(value)),
        };

        private static readonly Func<Optional<int>, Func<int, Optional<string>>, Task<Optional<string>>>[] PassMethodsTasks =
        {
            (optional, factory) => optional.Then(value => Task.FromResult(factory(value))),

            (optional, factory) => Task.FromResult(optional).Then(factory),
            (optional, factory) => Task.FromResult(optional).Then(value => Task.FromResult(factory(value))),
            (optional, factory) => Task.FromResult(optional).Then(value => ValueTaskFactory.Create(factory(value))),
            (optional, factory) => Task.FromResult(optional).Then(async value => factory(value)),

            (optional, factory) => ValueTaskFactory.Create(optional).Then(value => Task.FromResult(factory(value))),
        };

        private static IEnumerable<Func<Optional<int>, Func<int, Optional<string>>, Task<Optional<string>>>> PassMethods()
        {
            yield return (optional, factory) => Task.FromResult(optional.Then(factory));

            foreach (var method in PassMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in PassMethodsValueTasks)
            {
                yield return async (optional, factory) => await method(optional, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<Optional<int>, Func<Optional<string>>, ValueTask<Optional<string>>>[] FactoryMethodsValueTasks =
        {
            (optional, factory) => optional.Then(() => ValueTaskFactory.Create(factory())),
            (optional, factory) => optional.Then(async () => factory()),

            (optional, factory) => ValueTaskFactory.Create(optional).Then(factory),
            (optional, factory) => ValueTaskFactory.Create(optional).Then(() => ValueTaskFactory.Create(factory())),
            (optional, factory) => ValueTaskFactory.Create(optional).Then(async () => factory()),
        };

        private static readonly Func<Optional<int>, Func<Optional<string>>, Task<Optional<string>>>[] FactoryMethodsTasks =
        {
            (optional, factory) => optional.Then(() => Task.FromResult(factory())),

            (optional, factory) => Task.FromResult(optional).Then(factory),
            (optional, factory) => Task.FromResult(optional).Then(() => Task.FromResult(factory())),
            (optional, factory) => Task.FromResult(optional).Then(() => ValueTaskFactory.Create(factory())),
            (optional, factory) => Task.FromResult(optional).Then(async () => factory()),

            (optional, factory) => ValueTaskFactory.Create(optional).Then(() => Task.FromResult(factory())),
        };

        private static IEnumerable<Func<Optional<int>, Func<Optional<string>>, Task<Optional<string>>>> FactoryMethods()
        {
            yield return (optional, factory) => Task.FromResult(optional.Then(factory));

            foreach (var method in PassMethods())
            {
                yield return (optional, factory) => method(optional, _ => factory());
            }

            foreach (var method in FactoryMethodsTasks)
            {
                yield return method;
            }

            foreach (var method in FactoryMethodsValueTasks)
            {
                yield return async (optional, factory) => await method(optional, factory).ConfigureAwait(false);
            }
        }

        private static readonly Func<Optional<int>, Optional<string>, ValueTask<Optional<string>>>[] PlainMethodsValueTasks =
        {
            (optional1, optional2) => optional1.Then(ValueTaskFactory.Create(optional2)),

            (optional1, optional2) => ValueTaskFactory.Create(optional1).Then(optional2),
            (optional1, optional2) => ValueTaskFactory.Create(optional1).Then(ValueTaskFactory.Create(optional2)),
        };

        private static readonly Func<Optional<int>, Optional<string>, Task<Optional<string>>>[] PlainMethodsTasks =
        {
            (optional1, optional2) => optional1.Then(Task.FromResult(optional2)),

            (optional1, optional2) => Task.FromResult(optional1).Then(optional2),
            (optional1, optional2) => Task.FromResult(optional1).Then(Task.FromResult(optional2)),
            (optional1, optional2) => Task.FromResult(optional1).Then(ValueTaskFactory.Create(optional2)),

            (optional1, optional2) => ValueTaskFactory.Create(optional1).Then(Task.FromResult(optional2)),
        };

        private static IEnumerable<Func<Optional<int>, Optional<string>, Task<Optional<string>>>> AllMethods()
        {
            yield return (optional1, optional2) => Task.FromResult(optional1.Then(optional2));

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
            select new TestCaseData(testCase.Option1, testCase.Option2, method).Returns(testCase.Result);

        [TestCaseSource(nameof(Cases))]
        public async Task<Optional<string>> Process(
            Optional<int> optional1,
            Optional<string> optional2,
            Func<Optional<int>, Optional<string>, Task<Optional<string>>> then)
        {
            return await then(optional1, optional2).ConfigureAwait(false);
        }

        private static readonly IEnumerable<TestCaseData> PassValueCases = PassMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(PassValueCases))]
        public async Task Pass_Value_If_Some(Func<Optional<int>, Func<int, Optional<string>>, Task<Optional<string>>> then)
        {
            var optional = Optional<int>.Some(123);

            var result = await then(
                optional,
                value => Optional<string>.Some(value.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);

            result.Should().Be(Optional<string>.Some("123"));
        }

        private static Optional<string> AssertIsNotCalled()
        {
            Assert.Fail("Factory should not be called on None");
            throw new UnreachableException();
        }

        private static readonly IEnumerable<TestCaseData> AssertIsNotCalledCases =
            FactoryMethods().Select(method => new TestCaseData(method));

        [TestCaseSource(nameof(AssertIsNotCalledCases))]
        public async Task Do_Not_Call_Delegate_If_None(Func<Optional<int>, Func<Optional<string>>, Task<Optional<string>>> then)
        {
            var optional = Optional<int>.None();

            await then(optional, AssertIsNotCalled).ConfigureAwait(false);
        }
    }
}
