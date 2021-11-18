using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnFailure.TValue.Upcast
{
    [TestFixture]
    internal class Both_Should
    {
        private static readonly Func<Result<Child, Child>, Result<Base, Base>>[] UpcastMethods =
        {
            result => result.OnFailure<Base, Base>(_ => { }),
            result => result.OnFailure<Base, Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastTValueExamples.GetBoth()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<Base, Base> Return_Self_On_Upcast(
            Result<Child, Child> source,
            Func<Result<Child, Child>, Result<Base, Base>> callOnFailure)
        {
            return callOnFailure(source);
        }
    }
}
