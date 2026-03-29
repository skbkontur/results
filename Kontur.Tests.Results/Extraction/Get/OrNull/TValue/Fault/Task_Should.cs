using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrNull.TValue.Fault
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int?>> method,
            int? expected)
            where TResult : IResult<int, string>
        {
            return new(source, method) { ExpectedResult = expected };
        }

        private static readonly Func<Task<Result<int, string>>, Task<int?>>[] ResultMethods =
        {
            result => result.GetFaultOrNull(),
        };

        private static readonly Func<Task<IResult<int, string>>, Task<int?>>[] InterfaceMethods =
        {
            result => result.GetFaultOrNull(),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TResult>(
            TResult source,
            IEnumerable<Func<Task<TResult>, Task<int?>>> methods,
            int? expected)
            where TResult : IResult<int, string>
        {
            return methods.Select(method => CreateCase(source, method, expected));
        }

        private static readonly IEnumerable<TestCaseData> Cases =
            GenerateCases(Result<int, string>.Succeed("val"), ResultMethods, null)
                .Concat(GenerateCases(Result<int, string>.Succeed("val"), InterfaceMethods, null))
                .Concat(GenerateCases(Result<int, string>.Fail(5), ResultMethods, 5))
                .Concat(GenerateCases(Result<int, string>.Fail(5), InterfaceMethods, 5));

        [TestCaseSource(nameof(Cases))]
        public Task<int?> Return_Fault_Or_Null<TResult>(TResult result, Func<Task<TResult>, Task<int?>> callGetOrNull)
            where TResult : IResult<int, string>
        {
            var task = Task.FromResult(result);

            return callGetOrNull(task);
        }
    }
}
