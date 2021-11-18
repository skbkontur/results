using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain.SelectMany.Results2.Plain
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = FixtureCase.GenerateCases(2);

        private static Task<Result<string>> SelectResult(int value)
        {
            return Task.FromResult(GetResult(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string>> Result_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from y in result2
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string>> TaskResult_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in result2
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string>> Result_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from y in Task.FromResult(result2)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string>> TaskResult_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in Task.FromResult(result2)
                select SelectResult(x + y);
        }
    }
}
