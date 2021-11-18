using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain.SelectMany.Results1.Tasks2
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private const int TaskTerm1 = 1000;
        private const int TaskTerm2 = 10000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm1);
        private static readonly Task<int> Task10000 = Task.FromResult(TaskTerm2);

        private static readonly IEnumerable<TestCaseData> Cases =
            FixtureCase.GenerateCases(1, sum => sum + TaskTerm1 + TaskTerm2);

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string>> Result_Task_Task(Result<string, int> result)
        {
            return
                from x in result
                from y in Task1000
                from z in Task10000
                select GetResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string>> TaskResult_Task_Task(Result<string, int> result)
        {
            return
                from x in Task.FromResult(result)
                from y in Task1000
                from z in Task10000
                select GetResult(x + y + z);
        }
    }
}