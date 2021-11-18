using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.Select.SameType
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator.Create(1).ToTestCases();

        [TestCaseSource(nameof(Cases))]
        public Optional<int> OneOption(Optional<int> optional)
        {
            return
                from value in optional
                select value;
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Option_Let(Optional<int> optional)
        {
            return
                from valueLet in optional
                let value = valueLet
                select value;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption(Optional<int> optional)
        {
            return
                from value in Task.FromResult(optional)
                select value;
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Let(Optional<int> optional)
        {
            return
                from valueLet in Task.FromResult(optional)
                let value = valueLet
                select value;
        }
    }
}
