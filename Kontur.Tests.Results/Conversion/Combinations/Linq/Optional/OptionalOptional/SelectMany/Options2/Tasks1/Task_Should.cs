using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.SelectMany.Options2.Tasks1
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private const int TaskTerm = 1000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm);

        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(2, sum => sum + TaskTerm);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOption(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_Task(
            Optional<int> option1,
            Optional<int> option2)
        {
            return
                from x in option1
                from y in option2
                from z in Task1000
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Task(
            Optional<int> option1,
            Optional<int> option2)
        {
            return
                from x in Task.FromResult(option1)
                from y in option2
                from z in Task1000
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Task(
            Optional<int> option1,
            Optional<int> option2)
        {
            return
                from x in option1
                from y in Task.FromResult(option2)
                from z in Task1000
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Task(
            Optional<int> option1,
            Optional<int> option2)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task.FromResult(option2)
                from z in Task1000
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Task_Option(Optional<int> option1, Optional<int> option2)
        {
            return
                from x in option1
                from y in Task1000
                from z in option2
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Task_Option(Optional<int> option1, Optional<int> option2)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task1000
                from z in option2
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Task_TaskOption(Optional<int> option1, Optional<int> option2)
        {
            return
                from x in option1
                from y in Task1000
                from z in Task.FromResult(option2)
                select SelectResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Task_TaskOption(Optional<int> option1, Optional<int> option2)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task1000
                from z in Task.FromResult(option2)
                select SelectResult(x + y + z);
        }
    }
}