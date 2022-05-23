using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Where.Plain.SelectMany
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = FixtureCase.CreateWhereCases(Constant, 2);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOptional(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Optional_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                from y in optional2
                where isSuitable(x)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Optional_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                where isSuitable(x)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_TaskOptional_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                where isSuitable(x)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_TaskOptional_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                where isSuitable(x)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Where_Optional(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                where isSuitable(x)
                from y in optional2
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Where_Optional(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                where isSuitable(x)
                from y in optional2
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Where_TaskOptional(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                where isSuitable(x)
                from y in Task.FromResult(optional2)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Where_TaskOptional(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                where isSuitable(x)
                from y in Task.FromResult(optional2)
                select SelectResult(x + y);
        }
    }
}
