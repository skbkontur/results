using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.OnSuccess.Upcast
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly Func<StringFaultResult<Child>, Result<StringFault, Base>>[] UpcastMethods =
        {
            result => result.OnSuccess<StringFault, Base>(_ => { }),
            result => result.OnSuccess<StringFault, Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastExamples.GetValues()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<StringFault, Base> Return_Self_On_Upcast(
            StringFaultResult<Child> source,
            Func<StringFaultResult<Child>, Result<StringFault, Base>> callOnSuccess)
        {
            return callOnSuccess(source);
        }
    }
}
