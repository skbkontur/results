using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional.SelectMany.Options2.Plain
{
    internal class Value_Should<TFixtureCase> : LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private static readonly IEnumerable<TestCaseData> Cases = CreateSelectCases(2);

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Optional_Optional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from y in optional2
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Optional_Let_Optional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from y in optional2
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Optional_Optional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from yLet in optional2
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Optional_Let_Optional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from yLet in optional2
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Optional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Let_Optional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from y in optional2
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Optional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from yLet in optional2
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Let_Optional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from yLet in optional2
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_TaskOptional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Let_TaskOptional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from y in Task.FromResult(optional2)
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_TaskOptional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Optional_Let_TaskOptional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_TaskOptional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Let_TaskOptional(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from y in Task.FromResult(optional2)
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_TaskOptional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOptional(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOptional_Let_TaskOptional_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOptional(x + y);
        }
    }
}
