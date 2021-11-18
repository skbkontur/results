using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue.SelectMany.Results1.Tasks1
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private const int TaskTerm = 1000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm);

        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(1, sum => sum + TaskTerm);

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Task(Result<string, int> result)
        {
            return
                from x in result
                from y in Task1000
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Task(Result<string, int> result)
        {
            return
                from x in Task.FromResult(result)
                from y in Task1000
                select GetResult(x + y);
        }
    }
}