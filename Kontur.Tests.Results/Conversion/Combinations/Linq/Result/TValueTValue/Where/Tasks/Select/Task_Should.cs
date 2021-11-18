using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue.Where.Tasks.Select
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = FixtureCase.CreateWhereCases(Provider, 1, 1);

        private static Task<Result<string, int>> SelectResult(int value)
        {
            return Task.FromResult(GetResult(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> OneResult(Result<string, int> result, IsSuitable isSuitable)
        {
            return
                from value in result
                where Task.FromResult(isSuitable(value))
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult(Result<string, int> result, IsSuitable isSuitable)
        {
            return
                from value in Task.FromResult(result)
                where Task.FromResult(isSuitable(value))
                select SelectResult(value);
        }
    }
}
