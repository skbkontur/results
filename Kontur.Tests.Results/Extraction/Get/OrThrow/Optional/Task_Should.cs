using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Optional
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateThrowCase<TOptional>(
            TOptional source,
            Func<Task<TOptional>, Task<int>> method)
            where TOptional : IOptional<int>
        {
            return new(source, method);
        }

        private static readonly Func<Task<Optional<int>>, Task<int>>[] OptionalMethods =
        {
            optional => optional.GetValueOrThrow(() => new MyException()),
            optional => optional.GetValueOrThrow(new MyException()),
        };

        private static readonly Func<Task<IOptional<int>>, Task<int>>[] InterfaceMethods =
        {
            optional => optional.GetValueOrThrow(() => new MyException()),
            optional => optional.GetValueOrThrow(new MyException()),
        };

        private static IEnumerable<TestCaseData> GenerateThrowCases<TOptional>(
            TOptional none,
            IEnumerable<Func<Task<TOptional>, Task<int>>> methods)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => CreateThrowCase(none, method));
        }

        private static readonly IEnumerable<TestCaseData> ThrowCases =
            GenerateThrowCases(Optional<int>.None(), OptionalMethods)
                .Concat(GenerateThrowCases(Optional<int>.None(), InterfaceMethods));

        [TestCaseSource(nameof(ThrowCases))]
        public void Throw_MyException_If_None<TOptional>(TOptional optional, Func<Task<TOptional>, Task<int>> callGetOrThrow)
            where TOptional : IOptional<int>
        {
            var task = Task.FromResult(optional);

            Func<Task> action = () => callGetOrThrow(task);

            action.Should().ThrowAsync<MyException>();
        }

        private static TestCaseData CreateReturnCase<TOptional>(
            TOptional source,
            Func<Task<TOptional>, Task<int>> method)
            where TOptional : IOptional<int>
        {
            return new(source, method)
            {
                ExpectedResult = 5,
            };
        }

        private static IEnumerable<TestCaseData> GenerateReturnCases<TOptional>(
            TOptional some,
            IEnumerable<Func<Task<TOptional>, Task<int>>> methods)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => CreateReturnCase(some, method));
        }

        private static readonly IEnumerable<TestCaseData> ReturnCases =
            GenerateReturnCases(Optional<int>.Some(5), OptionalMethods)
                .Concat(GenerateReturnCases(Optional<int>.Some(5), InterfaceMethods));

        [TestCaseSource(nameof(ReturnCases))]
        public Task<int> Return_Value_If_Some<TOptional>(TOptional optional, Func<Task<TOptional>, Task<int>> callGetOrThrow)
            where TOptional : IOptional<int>
        {
            var task = Task.FromResult(optional);

            return callGetOrThrow(task);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on Some");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Optional<int>>, Task<int>>[] OptionalAssertNotCalledMethods =
        {
            optional => optional.GetValueOrThrow(AssertIsNotCalled),
        };

        private static readonly Func<Task<IOptional<int>>, Task<int>>[] InterfaceAssertNotCalledMethods =
        {
            optional => optional.GetValueOrThrow(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TOptional>(
            TOptional some,
            IEnumerable<Func<Task<TOptional>, Task<int>>> methods)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => new TestCaseData(some, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfSomeCases =
            CreateDoNotCallFactoryCases(Optional<int>.Some(5), OptionalAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Optional<int>.Some(5), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfSomeCases))]
        public async Task Do_Not_Call_Delegate_If_Some<TOptional>(TOptional optional, Func<Task<TOptional>, Task<int>> assertExtracted)
            where TOptional : IOptional<int>
        {
            var task = Task.FromResult(optional);

            await assertExtracted(task).ConfigureAwait(false);
        }

        private class MyException : Exception
        {
        }
    }
}
