using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.Where.Plain.Select
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = WhereCaseGenerator.Create(1);

        [TestCaseSource(nameof(Cases))]
        public Optional<int> OneOption(Optional<int> optional, IsSuitable isSuitable)
        {
            return
                from value in optional
                where isSuitable(value)
                select value;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption(Optional<int> optional, IsSuitable isSuitable)
        {
            return
                from value in Task.FromResult(optional)
                where isSuitable(value)
                select value;
        }
    }
}
