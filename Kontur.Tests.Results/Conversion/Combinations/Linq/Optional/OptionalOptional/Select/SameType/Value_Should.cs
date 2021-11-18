using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Select.SameType
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(1);

        [TestCaseSource(nameof(Cases))]
        public Optional<int> OneOption(Optional<int> optional)
        {
            return
                from value in optional
                select GetOption(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Option_Let(Optional<int> optional)
        {
            return
                from valueLet in optional
                let value = valueLet
                select GetOption(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption(Optional<int> optional)
        {
            return
                from value in Task.FromResult(optional)
                select GetOption(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Let(Optional<int> optional)
        {
            return
                from valueLet in Task.FromResult(optional)
                let value = valueLet
                select GetOption(value);
        }
    }
}
