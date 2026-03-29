using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrNull.Optional
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateCase<TOptional>(
            TOptional source,
            Func<Task<TOptional>, Task<int?>> method,
            int? expected)
            where TOptional : IOptional<int>
        {
            return new(source, method) { ExpectedResult = expected };
        }

        private static readonly Func<Task<Optional<int>>, Task<int?>>[] OptionalMethods =
        {
            optional => optional.GetValueOrNull(),
        };

        private static readonly Func<Task<IOptional<int>>, Task<int?>>[] InterfaceMethods =
        {
            optional => optional.GetValueOrNull(),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TOptional>(
            TOptional source,
            IEnumerable<Func<Task<TOptional>, Task<int?>>> methods,
            int? expected)
            where TOptional : IOptional<int>
        {
            return methods.Select(method => CreateCase(source, method, expected));
        }

        private static readonly IEnumerable<TestCaseData> Cases =
            GenerateCases(Optional<int>.None(), OptionalMethods, null)
                .Concat(GenerateCases(Optional<int>.None(), InterfaceMethods, null))
                .Concat(GenerateCases(Optional<int>.Some(5), OptionalMethods, 5))
                .Concat(GenerateCases(Optional<int>.Some(5), InterfaceMethods, 5));

        [TestCaseSource(nameof(Cases))]
        public Task<int?> Return_Value_Or_Null<TOptional>(TOptional optional, Func<Task<TOptional>, Task<int?>> callGetOrNull)
            where TOptional : IOptional<int>
        {
            var task = Task.FromResult(optional);

            return callGetOrNull(task);
        }
    }
}
