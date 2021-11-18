using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.Where.Tasks.Select
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = WhereCaseGenerator.Create(1, 1);

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> OneResult(Result<string, int> result, IsSuitable isSuitable)
        {
            return
                from value in result
                where Task.FromResult(isSuitable(value))
                select Task.FromResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult(Result<string, int> result, IsSuitable isSuitable)
        {
            return
                from value in Task.FromResult(result)
                where Task.FromResult(isSuitable(value))
                select Task.FromResult(value);
        }
    }
}
