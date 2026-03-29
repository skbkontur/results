using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrNull.TValue.Value
{
    [TestFixture]
    internal class Task_Should
    {
        private static TestCaseData CreateCase<TResult>(
            TResult source,
            Func<Task<TResult>, Task<int?>> method,
            int? expected)
            where TResult : IResult<string, int>
        {
            return new(source, method) { ExpectedResult = expected };
        }

        private static readonly Func<Task<Result<string, int>>, Task<int?>>[] ResultMethods =
        {
            result => result.GetValueOrNull(),
        };

        private static readonly Func<Task<IResult<string, int>>, Task<int?>>[] InterfaceMethods =
        {
            result => result.GetValueOrNull(),
        };

        private static IEnumerable<TestCaseData> GenerateCases<TResult>(
            TResult source,
            IEnumerable<Func<Task<TResult>, Task<int?>>> methods,
            int? expected)
            where TResult : IResult<string, int>
        {
            return methods.Select(method => CreateCase(source, method, expected));
        }

        private static readonly IEnumerable<TestCaseData> Cases =
            GenerateCases(Result<string, int>.Fail("err"), ResultMethods, null)
                .Concat(GenerateCases(Result<string, int>.Fail("err"), InterfaceMethods, null))
                .Concat(GenerateCases(Result<string, int>.Succeed(5), ResultMethods, 5))
                .Concat(GenerateCases(Result<string, int>.Succeed(5), InterfaceMethods, 5));

        [TestCaseSource(nameof(Cases))]
        public Task<int?> Return_Value_Or_Null<TResult>(TResult result, Func<Task<TResult>, Task<int?>> callGetOrNull)
            where TResult : IResult<string, int>
        {
            var task = Task.FromResult(result);

            return callGetOrNull(task);
        }
    }
}
