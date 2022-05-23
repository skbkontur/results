﻿using System.Collections.Generic;
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
        public Optional<int> Option_Option(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from y in optional2
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Option_Let_Option(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from y in optional2
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Option_Option_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from yLet in optional2
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Option_Let_Option_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from yLet in optional2
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Let_Option(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from y in optional2
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from yLet in optional2
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Let_Option_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from yLet in optional2
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Let_TaskOption(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from y in Task.FromResult(optional2)
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in optional1
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Let_TaskOption_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in optional1
                let x = xLet
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Let_TaskOption(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from y in Task.FromResult(optional2)
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from x in Task.FromResult(optional1)
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOption(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Let_TaskOption_Let(Optional<int> optional1, Optional<int> optional2)
        {
            return
                from xLet in Task.FromResult(optional1)
                let x = xLet
                from yLet in Task.FromResult(optional2)
                let y = yLet
                select GetOption(x + y);
        }
    }
}
