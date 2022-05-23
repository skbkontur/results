using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.SelectMany.Options2.Tasks1
{
    [TestFixture]
    internal class Task_Should
    {
        private const int TaskTerm = 1000;
        private static readonly Task<int> Task1000 = Task.FromResult(TaskTerm);

        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator
            .Create(2)
            .ToTestCases(option => option.MapValue(sum => sum + TaskTerm));

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Optional_Task(
            Optional<int> optional1,
            Optional<int> optional2)
        {
            return
                from x in optional1
                from y in optional2
                from z in Task1000
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Optional_Task(
            Optional<int> optional1,
            Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                from z in Task1000
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_TaskOptional_Task(
            Optional<int> optional1,
            Optional<int> optional2)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                from z in Task1000
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_TaskOptional_Task(
            Optional<int> optional1,
            Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                from z in Task1000
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Task_Optional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from y in Task1000
                from z in optional2
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Task_Optional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task1000
                from z in optional2
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Task_TaskOptional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from y in Task1000
                from z in Task.FromResult(optional2)
                select Task.FromResult(x + y + z);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Task_TaskOptional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task1000
                from z in Task.FromResult(optional2)
                select Task.FromResult(x + y + z);
        }
    }
}