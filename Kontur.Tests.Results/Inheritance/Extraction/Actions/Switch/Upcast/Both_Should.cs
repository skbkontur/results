using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.Switch.Upcast
{
    [TestFixture]
    internal class Both_Should
    {
        private static readonly Func<StringFaultResult<Child>, Result<StringFaultBase, Base>>[] UpcastMethods =
        {
            result => result.Switch<StringFaultBase, Base>(_ => { }, () => { }),
            result => result.Switch<StringFaultBase, Base>(() => { }, () => { }),
            result => result.Switch<StringFaultBase, Base>(_ => { }, _ => { }),
            result => result.Switch<StringFaultBase, Base>(() => { }, _ => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastExamples.GetBoth()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<StringFaultBase, Base> Return_Self_On_Upcast(
            StringFaultResult<Child> source,
            Func<StringFaultResult<Child>, Result<StringFaultBase, Base>> callSwitch)
        {
            return callSwitch(source);
        }
    }
}
