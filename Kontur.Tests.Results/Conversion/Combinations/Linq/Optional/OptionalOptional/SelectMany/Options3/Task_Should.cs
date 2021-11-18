using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.SelectMany.Options3
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(3);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOption(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in option2
                from z in option3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in option2
                from z in option3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in Task.FromResult(option2)
                from z in option3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in option2
                from z in Task.FromResult(option3)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task.FromResult(option2)
                from z in option3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in option2
                from z in Task.FromResult(option3)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in Task.FromResult(option2)
                from z in Task.FromResult(option3)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task.FromResult(option2)
                from z in Task.FromResult(option3)
                select SelectResult(x + y + z);
        }
    }
}
