﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.Optional.Where.Tasks.SelectMany
{
    [TestFixture]
    internal class Task_Should
    {
        private static readonly IEnumerable<TestCaseData> Cases = WhereCaseGenerator.Create(2);

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Option_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                from y in optional2
                where Task.FromResult(isSuitable(x))
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Option_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                from y in optional2
                where Task.FromResult(isSuitable(x))
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_TaskOption_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                from y in Task.FromResult(optional2)
                where Task.FromResult(isSuitable(x))
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_TaskOption_Where(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                from y in Task.FromResult(optional2)
                where Task.FromResult(isSuitable(x))
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Where_Option(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                where Task.FromResult(isSuitable(x))
                from y in optional2
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Where_Option(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                where Task.FromResult(isSuitable(x))
                from y in optional2
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> Option_Where_TaskOption(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in optional1
                where Task.FromResult(isSuitable(x))
                from y in Task.FromResult(optional2)
                select Task.FromResult(x + y);
        }

        [TestCaseSource(nameof(Cases))]
        public Task<Optional<int>> TaskOption_Where_TaskOption(Optional<int> optional1, Optional<int> optional2, IsSuitable isSuitable)
        {
            return
                from x in Task.FromResult(optional1)
                where Task.FromResult(isSuitable(x))
                from y in Task.FromResult(optional2)
                select Task.FromResult(x + y);
        }
    }
}
