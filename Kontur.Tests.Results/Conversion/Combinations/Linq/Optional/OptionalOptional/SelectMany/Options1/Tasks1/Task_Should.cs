using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.SelectMany.Options1.Tasks1
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private const int TaskTerm = 1000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm);

        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(1, sum => sum + TaskTerm);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOption(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Task(Optional<int> optional)
        {
            return
                from x in optional
                from y in Task1000
                select SelectResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Task(Optional<int> optional)
        {
            return
                from x in Task.FromResult(optional)
                from y in Task1000
                select SelectResult(x + y);
        }
    }
}