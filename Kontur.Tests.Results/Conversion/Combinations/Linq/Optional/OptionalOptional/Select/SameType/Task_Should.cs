using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Select.SameType
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(1);

        private static Task<Optional<int>> SelectResult(int value)
        {
            return Task.FromResult(GetOptional(value));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> OneOptional(Optional<int> optional)
        {
            return
                from value in optional
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Let(Optional<int> optional)
        {
            return
                from valueLet in optional
                let value = valueLet
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional(Optional<int> optional)
        {
            return
                from value in Task.FromResult(optional)
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Let(Optional<int> optional)
        {
            return
                from valueLet in Task.FromResult(optional)
                let value = valueLet
                select SelectResult(value);
        }
    }
}
