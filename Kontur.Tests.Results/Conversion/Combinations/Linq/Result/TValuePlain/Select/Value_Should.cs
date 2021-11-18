using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain.Select
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> FailureCases = FixtureCase.GenerateCases(1);

        [TestCaseSource(nameof(FailureCases))]
        public Result<string> OneResult(Result<string, int> result)
        {
            return
                from value in result
                select GetResult(value);
        }

        [TestCaseSource(nameof(FailureCases))]
        public Task<Result<string>> TaskResult(Result<string, int> result)
        {
            return
                from value in Task.FromResult(result)
                select GetResult(value);
        }
    }
}
