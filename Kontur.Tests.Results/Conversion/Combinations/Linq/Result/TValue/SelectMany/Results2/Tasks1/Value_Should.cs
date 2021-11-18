using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.SelectMany.Results2.Tasks1
{
    [TestFixture]
    internal class Value_Should
    {
        private const int TaskTerm = 1000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm);

        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator
            .Create(2)
            .ToTestCases(result => result.MapValue(sum => sum + TaskTerm));

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Result_Task(
            Result<string, int> result1,
            Result<string, int> result2)
        {
            return
                from x in result1
                from y in result2
                from z in Task1000
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Result_Task(
            Result<string, int> result1,
            Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in result2
                from z in Task1000
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_TaskResult_Task(
            Result<string, int> result1,
            Result<string, int> result2)
        {
            return
                from x in result1
                from y in Task.FromResult(result2)
                from z in Task1000
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_TaskResult_Task(
            Result<string, int> result1,
            Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in Task.FromResult(result2)
                from z in Task1000
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Task_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from y in Task1000
                from z in result2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Task_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in Task1000
                from z in result2
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Task_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from y in Task1000
                from z in Task.FromResult(result2)
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Task_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in Task1000
                from z in Task.FromResult(result2)
                select x + y + z;
        }
    }
}