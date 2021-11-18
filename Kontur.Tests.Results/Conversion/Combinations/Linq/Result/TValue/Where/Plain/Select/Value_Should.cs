using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValue.Where.Plain.Select
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = WhereCaseGenerator.Create(1, 1);

        [TestCaseSource(nameof(Cases))]
        public Result<string, int> OneResult(Result<string, int> result, IsSuitable isSuitable)
        {
            return
                from value in result
                where isSuitable(value)
                select value;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Result<string, int>> TaskResult(Result<string, int> result, IsSuitable isSuitable)
        {
            return
                from value in Task.FromResult(result)
                where isSuitable(value)
                select value;
        }
    }
}
