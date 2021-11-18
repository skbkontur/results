using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.Switch.Upcast
{
    [TestFixture]
    internal class Fault_Should
    {
        private static readonly Func<StringFaultResult<string>, Result<StringFaultBase, string>>[] UpcastMethods =
        {
            result => result.Switch<StringFaultBase, string>(_ => { }, () => { }),
            result => result.Switch<StringFaultBase, string>(() => { }, () => { }),
            result => result.Switch<StringFaultBase, string>(_ => { }, _ => { }),
            result => result.Switch<StringFaultBase, string>(() => { }, _ => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastExamples.GetFaults()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<StringFaultBase, string> Return_Self_On_Upcast(
            StringFaultResult<string> source,
            Func<StringFaultResult<string>, Result<StringFaultBase, string>> callSwitch)
        {
            return callSwitch(source);
        }
    }
}
