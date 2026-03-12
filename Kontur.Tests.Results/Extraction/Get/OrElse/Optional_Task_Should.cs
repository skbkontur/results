using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrElse
{
    [TestFixture]
    internal class Optional_Task_Should
    {
        private const string ExpectedValue = "fallback";

        private static TestCaseData CreateConvertCase<TOptional>(
            TOptional source,
            string expectedResult,
            Func<Task<TOptional>, Task<string>> method)
            where TOptional : IOptional<string>
        {
            return new(source, method)
            {
                ExpectedResult = expectedResult,
            };
        }

        private static readonly Func<Task<Optional<string>>, Task<string>>[] OptionalMethods =
        {
            optional => optional.GetValueOrElse(() => ExpectedValue),
            optional => optional.GetValueOrElse(ExpectedValue),
        };

        private static readonly Func<Task<IOptional<string>>, Task<string>>[] InterfaceMethods =
        {
            optional => optional.GetValueOrElse(() => ExpectedValue),
            optional => optional.GetValueOrElse(ExpectedValue),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TOptional>(
            TOptional none,
            TOptional some,
            string someValue,
            IEnumerable<Func<Task<TOptional>, Task<string>>> methods)
            where TOptional : IOptional<string>
        {
            return from testCase in new[]
                {
                    (Source: none, Result: ExpectedValue),
                    (Source: some, Result: someValue),
                }
                from method in methods
                select CreateConvertCase(testCase.Source, testCase.Result, method);
        }

        private static readonly IEnumerable<TestCaseData> ConvertCases =
            GenerateCases(Optional<string>.None(), Optional<string>.Some("bar"), "bar", OptionalMethods)
                .Concat(GenerateCases(Optional<string>.None(), Optional<string>.Some("bar"), "bar", InterfaceMethods));

        [TestCaseSource(nameof(ConvertCases))]
        public Task<string> Return_Result<TOptional>(TOptional optional, Func<Task<TOptional>, Task<string>> callGetOrElse)
            where TOptional : IOptional<string>
        {
            var task = Task.FromResult(optional);

            return callGetOrElse(task);
        }

        private static string AssertIsNotCalled()
        {
            Assert.Fail("Backup value factory should not be called on Some");
            throw new UnreachableException();
        }

        private static readonly Func<Task<Optional<string>>, Task<string>>[] OptionalAssertNotCalledMethods =
        {
            optional => optional.GetValueOrElse(AssertIsNotCalled),
        };

        private static readonly Func<Task<IOptional<string>>, Task<string>>[] InterfaceAssertNotCalledMethods =
        {
            optional => optional.GetValueOrElse(AssertIsNotCalled),
        };

        private static IEnumerable<TestCaseData> CreateDoNotCallFactoryCases<TOptional>(
            TOptional some,
            IEnumerable<Func<Task<TOptional>, Task<string>>> methods)
            where TOptional : IOptional<string>
        {
            return methods.Select(method => new TestCaseData(some, method));
        }

        private static readonly IEnumerable<TestCaseData> DoNotCallFactoryIfSomeCases =
            CreateDoNotCallFactoryCases(Optional<string>.Some("value"), OptionalAssertNotCalledMethods)
                .Concat(CreateDoNotCallFactoryCases(Optional<string>.Some("value"), InterfaceAssertNotCalledMethods));

        [TestCaseSource(nameof(DoNotCallFactoryIfSomeCases))]
        public async Task Do_Not_Call_Delegate_If_Some<TOptional>(TOptional optional, Func<Task<TOptional>, Task<string>> assertExtracted)
            where TOptional : IOptional<string>
        {
            var task = Task.FromResult(optional);

            await assertExtracted(task).ConfigureAwait(false);
        }
    }
}
