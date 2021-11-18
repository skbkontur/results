using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.SelectMany.Options3
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator.Create(3).ToTestCases();

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Option_Option_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in option2
                from z in option3
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in option2
                from z in option3
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in Task.FromResult(option2)
                from z in option3
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in option2
                from z in Task.FromResult(option3)
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Option(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task.FromResult(option2)
                from z in option3
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in option2
                from z in Task.FromResult(option3)
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in option1
                from y in Task.FromResult(option2)
                from z in Task.FromResult(option3)
                select x + y + z;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_TaskOption(Optional<int> option1, Optional<int> option2, Optional<int> option3)
        {
            return
                from x in Task.FromResult(option1)
                from y in Task.FromResult(option2)
                from z in Task.FromResult(option3)
                select x + y + z;
        }
    }
}
