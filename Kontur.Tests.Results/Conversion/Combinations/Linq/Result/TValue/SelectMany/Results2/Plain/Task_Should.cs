using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.SelectMany.Results2.Plain
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator.Create(2).ToTestCases();

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from y in result2
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Let_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in result1
                let x = xLet
                from y in result2
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Result_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from yLet in result2
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Let_Result_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in result1
                let x = xLet
                from yLet in result2
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in result2
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Let_Result(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in Task.FromResult(result1)
                let x = xLet
                from y in result2
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Result_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from yLet in result2
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Let_Result_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in Task.FromResult(result1)
                let x = xLet
                from yLet in result2
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from y in Task.FromResult(result2)
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Let_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in result1
                let x = xLet
                from y in Task.FromResult(result2)
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_TaskResult_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in result1
                from yLet in Task.FromResult(result2)
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Let_TaskResult_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in result1
                let x = xLet
                from yLet in Task.FromResult(result2)
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from y in Task.FromResult(result2)
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Let_TaskResult(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in Task.FromResult(result1)
                let x = xLet
                from y in Task.FromResult(result2)
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_TaskResult_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from x in Task.FromResult(result1)
                from yLet in Task.FromResult(result2)
                let y = yLet
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Let_TaskResult_Let(Result<string, int> result1, Result<string, int> result2)
        {
            return
                from xLet in Task.FromResult(result1)
                let x = xLet
                from yLet in Task.FromResult(result2)
                let y = yLet
                select Task.FromResult(x + y);
        }
    }
}
