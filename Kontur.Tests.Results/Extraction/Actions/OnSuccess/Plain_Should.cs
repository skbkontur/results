using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnSuccess
{
    [TestFixture]
    public class Plain_Should
    {
        [Test]
        public void Call_OnSuccess_If_Success()
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<string>.Succeed();

            result.OnSuccess(() => counter.Increment());

            counter.Received().Increment();
        }

        [Test]
        public void Do_Not_Call_OnSuccess_If_Failure()
        {
            var result = Result<string>.Fail("foo");

            result.OnSuccess(() => Assert.Fail("OnSuccess is called"));
        }

        private static TestCaseData CreateReturnSelfCase(Result<int> result)
        {
            return new(result) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] ReturnSelfCases =
        {
            CreateReturnSelfCase(Result<int>.Succeed()),
            CreateReturnSelfCase(Result<int>.Fail(1)),
        };

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<int> Return_Self_OnSuccess(Result<int> result)
        {
            return result.OnSuccess(() => { });
        }

        private static readonly Func<Result<Child>, Result<Base>>[] UpcastMethods =
        {
            result => result.OnSuccess<Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastPlainExamples.Get()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<Base> Return_Self_On_Upcast(
            Result<Child> source,
            Func<Result<Child>, Result<Base>> callOnSuccess)
        {
            return callOnSuccess(source);
        }
    }
}