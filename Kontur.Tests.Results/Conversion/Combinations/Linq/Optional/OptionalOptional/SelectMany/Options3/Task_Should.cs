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
        public Task<Optional<int>> Option_Option_Option(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in optional1
                from y in optional2
                from z in optional3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Option(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                from z in optional3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Option(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                from z in optional3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_TaskOption(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in optional1
                from y in optional2
                from z in Task.FromResult(optional3)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Option(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                from z in optional3
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_TaskOption(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                from z in Task.FromResult(optional3)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_TaskOption(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                from z in Task.FromResult(optional3)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_TaskOption(Optional<int> optional1, Optional<int> optional2, Optional<int> optional3)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                from z in Task.FromResult(optional3)
                select SelectResult(x + y + z);
        }
    }
}
