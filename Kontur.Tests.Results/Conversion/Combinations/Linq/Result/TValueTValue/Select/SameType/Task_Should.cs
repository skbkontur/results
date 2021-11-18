using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue.Select.SameType
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(1);

        private static Task<Result<string, int>> SelectResult(int value)
        {
            return Task.FromResult(GetResult(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> OneResult(Result<string, int> result)
        {
            return
                from value in result
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> Result_Let(Result<string, int> result)
        {
            return
                from valueLet in result
                let value = valueLet
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult(Result<string, int> result)
        {
            return
                from value in Task.FromResult(result)
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult_Let(Result<string, int> result)
        {
            return
                from valueLet in Task.FromResult(result)
                let value = valueLet
                select SelectResult(value);
        }
    }
}
