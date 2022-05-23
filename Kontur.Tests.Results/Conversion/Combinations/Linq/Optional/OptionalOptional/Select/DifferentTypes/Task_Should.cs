using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.Select.DifferentTypes
{
    internal class Task_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(1);

        private static Task<Optional<string>> SelectResult(int value)
        {
            return Task.FromResult(GetOptional(ConvertToString(value)));
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<string>> OneOptional(Optional<int> optional)
        {
            return
                from value in optional
                select SelectResult(value);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<string>> Optional_Let(Optional<int> optional)
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
