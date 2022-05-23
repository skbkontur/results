using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.SelectMany.Options1.Tasks2
{
    [TestFixture]
    internal class Task_Should
    {
        private const int TaskTerm1 = 1000;
        private const int TaskTerm2 = 10000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm1);
        private static readonly Task<int> Task10000 = Task.FromResult(TaskTerm2);

        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator
            .Create(1)
            .ToTestCases(option => option.MapValue(sum => sum + TaskTerm1 + TaskTerm2));

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Task_Task(Optional<int> optional)
        {
            return
                from x in optional
                from y in Task1000
                from z in Task10000
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Task_Task(Optional<int> optional)
        {
            return
                from x in Task.FromResult(optional)
                from y in Task1000
                from z in Task10000
                select Task.FromResult(x + y + z);
        }
    }
}