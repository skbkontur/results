using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.Switch.TValue.Upcast
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly Func<Result<string, Child>, Result<string, Base>>[] UpcastMethods =
        {
            result => result.Switch<string, Base>(_ => { }, () => { }),
            result => result.Switch<string, Base>(() => { }, () => { }),
            result => result.Switch<string, Base>(_ => { }, _ => { }),
            result => result.Switch<string, Base>(() => { }, _ => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastTValueExamples.GetValues()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<string, Base> Return_Self_On_Upcast(
            Result<string, Child> source,
            Func<Result<string, Child>, Result<string, Base>> callSwitch)
        {
            return callSwitch(source);
        }
    }
}
