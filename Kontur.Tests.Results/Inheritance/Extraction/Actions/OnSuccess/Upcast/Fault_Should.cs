using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.OnSuccess.Upcast
{
    [TestFixture]
    internal class Fault_Should
    {
        private static readonly Func<StringFaultResult<string>, Result<StringFaultBase, string>>[] UpcastMethods =
        {
            result => result.OnSuccess<StringFaultBase, string>(_ => { }),
            result => result.OnSuccess<StringFaultBase, string>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastExamples.GetFaults()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<StringFaultBase, string> Return_Self_On_Upcast(
            StringFaultResult<string> source,
            Func<StringFaultResult<string>, Result<StringFaultBase, string>> callOnSuccess)
        {
            return callOnSuccess(source);
        }
    }
}
