using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Where.Tasks.Select
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = FixtureCase.CreateWhereCases(Constant, 1);

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> OneOption(Optional<int> optional, IsSuitable isSuitable)
        {
            return
                from value in optional
                where Task.FromResult(isSuitable(value))
                select GetOption(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption(Optional<int> optional, IsSuitable isSuitable)
        {
            return
                from value in Task.FromResult(optional)
                where Task.FromResult(isSuitable(value))
                select GetOption(value);
        }
    }
}
