using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnSuccess.TValue.Upcast
{
    [TestFixture]
    internal class Value_Should
    {
        private static readonly Func<Result<string, Child>, Result<string, Base>>[] UpcastMethods =
        {
            result => result.OnSuccess<string, Base>(_ => { }),
            result => result.OnSuccess<string, Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastTValueExamples.GetValues()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<string, Base> Return_Self_On_Upcast(
            Result<string, Child> source,
            Func<Result<string, Child>, Result<string, Base>> callOnSuccess)
        {
            return callOnSuccess(source);
        }
    }
}
