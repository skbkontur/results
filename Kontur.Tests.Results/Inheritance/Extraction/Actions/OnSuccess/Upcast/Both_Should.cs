using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.OnSuccess.Upcast
{
    [TestFixture]
    internal class Both_Should
    {
        private static readonly Func<StringFaultResult<Child>, Result<StringFaultBase, Base>>[] UpcastMethods =
        {
            result => result.OnSuccess<StringFaultBase, Base>(_ => { }),
            result => result.OnSuccess<StringFaultBase, Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastExamples.GetBoth()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<StringFaultBase, Base> Return_Self_On_Upcast(
            StringFaultResult<Child> source,
            Func<StringFaultResult<Child>, Result<StringFaultBase, Base>> callOnSuccess)
        {
            return callOnSuccess(source);
        }
    }
}
