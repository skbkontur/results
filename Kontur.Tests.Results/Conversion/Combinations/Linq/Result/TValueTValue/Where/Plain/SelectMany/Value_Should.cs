using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue.Where.Plain.SelectMany
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases2 = FixtureCase.CreateWhereCases(Provider, 2, 2);

        [TestCaseSource(nameof(Cases2))]
        public Result<string, int> Result_Result_Where(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in result1
                from y in result2
                where isSuitable(x)
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases2))]
        public Task<Result<string, int>> TaskResult_Result_Where(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(result1)
                from y in result2
                where isSuitable(x)
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases2))]
        public Task<Result<string, int>> Result_TaskResult_Where(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in result1
                from y in Task.FromResult(result2)
                where isSuitable(x)
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases2))]
        public Task<Result<string, int>> TaskResult_TaskResult_Where(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(result1)
                from y in Task.FromResult(result2)
                where isSuitable(x)
                select GetResult(x + y);
        }

        private static readonly IEnumerable<TestCaseData> Cases1 = FixtureCase.CreateWhereCases(Provider, 2, 1);

        [TestCaseSource(nameof(Cases1))]
        public Result<string, int> Result_Where_Result(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in result1
                where isSuitable(x)
                from y in result2
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases1))]
        public Task<Result<string, int>> TaskResult_Where_Result(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(result1)
                where isSuitable(x)
                from y in result2
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases1))]
        public Task<Result<string, int>> Result_Where_TaskResult(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in result1
                where isSuitable(x)
                from y in Task.FromResult(result2)
                select GetResult(x + y);
        }

        [TestCaseSource(nameof(Cases1))]
        public Task<Result<string, int>> TaskResult_Where_TaskResult(
            Result<string, int> result1,
            Result<string, int> result2,
            IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(result1)
                where isSuitable(x)
                from y in Task.FromResult(result2)
                select GetResult(x + y);
        }
    }
}