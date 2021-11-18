using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.Select.SameType
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator.Create(1).ToTestCases();

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> OneResult(Result<string, int> result)
        {
            return
                from value in result
                select Task.FromResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Let(Result<string, int> result)
        {
            return
                from valueLet in result
                let value = valueLet
                select Task.FromResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult(Result<string, int> result)
        {
            return
                from value in Task.FromResult(result)
                select Task.FromResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Let(Result<string, int> result)
        {
            return
                from valueLet in Task.FromResult(result)
                let value = valueLet
                select Task.FromResult(value);
        }
    }
}
