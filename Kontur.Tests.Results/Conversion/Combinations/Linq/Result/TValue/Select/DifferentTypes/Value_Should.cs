using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.Select.DifferentTypes
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator.Create(1).ToDifferentTypeTestCases();

        private static string SelectResult(int value)
        {
            return SelectCaseToDifferentTypeTestCasesExtensions.ConvertToString(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Result<string, string> OneResult(Result<string, int> result)
        {
            return
                from value in result
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Result<string, string> Result_Let(Result<string, int> result)
        {
            return
                from valueLet in result
                let value = valueLet
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, string>> TaskResult(Result<string, int> result)
        {
            return
                from value in Task.FromResult(result)
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, string>> TaskResult_Let(Result<string, int> result)
        {
            return
                from valueLet in Task.FromResult(result)
                let value = valueLet
                select SelectResult(value);
        }
    }
}
