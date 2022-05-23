using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Where.Plain.Select
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = FixtureCase.CreateWhereCases(Constant, 1);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOptional(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> OneOptional(Optional<int> optional, IsSuitable isSuitable)
        {
            return
                from value in optional
                where isSuitable(value)
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional(Optional<int> optional, IsSuitable isSuitable)
        {
            return
                from value in Task.FromResult(optional)
                where isSuitable(value)
                select SelectResult(value);
        }
    }
}
