using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.Select.DifferentTypes
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = SelectCasesGenerator.Create(1).ToDifferentTypeTestCases();

        private static string SelectResult(int value)
        {
            return SelectCaseToDifferentTypeTestCasesExtensions.ConvertToString(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<string> OneOptional(Optional<int> optional)
        {
            return
                from value in optional
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<string> Optional_Let(Optional<int> optional)
        {
            return
                from valueLet in optional
                let value = valueLet
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<string>> TaskOptional(Optional<int> optional)
        {
            return
                from value in Task.FromResult(optional)
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<string>> TaskOptional_Let(Optional<int> optional)
        {
            return
                from valueLet in Task.FromResult(optional)
                let value = valueLet
                select SelectResult(value);
        }
    }
}
