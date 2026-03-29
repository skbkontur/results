using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrNull.Plain
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int?>> method,
            int? expected)
            where TResult : IResult<int>
        {
            return new(source, method) { ExpectedResult = expected };
        }

        private static readonly Func<Task<Result<int>>, Task<int?>>[] ResultMethods =
        {
            result => result.GetFaultOrNull(),
        };

        private static readonly Func<Task<IResult<int>>, Task<int?>>[] InterfaceMethods =
        {
            result => result.GetFaultOrNull(),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TResult>(
            TResult source,
            IEnumerable<Func<Task<TResult>, Task<int?>>> methods,
            int? expected)
            where TResult : IResult<int>
        {
            return methods.Select(method => CreateCase(source, method, expected));
        }

        private static readonly IEnumerable<TestCaseData> Cases =
            GenerateCases(Result<int>.Succeed(), ResultMethods, null)
                .Concat(GenerateCases(Result<int>.Succeed(), InterfaceMethods, null))
                .Concat(GenerateCases(Result<int>.Fail(5), ResultMethods, 5))
                .Concat(GenerateCases(Result<int>.Fail(5), InterfaceMethods, 5));

        [TestCaseSource(nameof(Cases))]
        public Task<int?> Return_Fault_Or_Null<TResult>(TResult result, Func<Task<TResult>, Task<int?>> callGetOrNull)
            where TResult : IResult<int>
        {
            var task = Task.FromResult(result);

            return callGetOrNull(task);
        }
    }
}
