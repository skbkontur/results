using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Where.Tasks.SelectMany
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = FixtureCase.CreateWhereCases(Constant, 2);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOption(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_Where(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in option1
                from y in option2
                where Task.FromResult(isSuitable(x))
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Where(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(option1)
                from y in option2
                where Task.FromResult(isSuitable(x))
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Where(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in option1
                from y in Task.FromResult(option2)
                where Task.FromResult(isSuitable(x))
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Where(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task.FromResult(option2)
                where Task.FromResult(isSuitable(x))
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Where_Option(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in option1
                where Task.FromResult(isSuitable(x))
                from y in option2
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Where_Option(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(option1)
                where Task.FromResult(isSuitable(x))
                from y in option2
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Where_TaskOption(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in option1
                where Task.FromResult(isSuitable(x))
                from y in Task.FromResult(option2)
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Where_TaskOption(Optional<int> option1, Optional<int> option2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(option1)
                where Task.FromResult(isSuitable(x))
                from y in Task.FromResult(option2)
                select SelectResult(x + y);
        }
    }
}
