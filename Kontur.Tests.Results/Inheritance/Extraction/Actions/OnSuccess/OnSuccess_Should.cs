using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.OnSuccess
{
    [TestFixture]
    public class OnSuccess_Should
    {
        private static TestCaseData CreateCallOnSuccessIfSuccessCase(Func<StringFaultResult<Guid>, ICounter, Result<StringFault, Guid>> callOnSuccess)
        {
            return new(callOnSuccess);
        }

        private static readonly TestCaseData[] CallOnSuccessIfSuccessCases =
        {
            CreateCallOnSuccessIfSuccessCase((result, counter) => result.OnSuccess(counter.Increment)),
            CreateCallOnSuccessIfSuccessCase((result, counter) => result.OnSuccess(_ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public void Call_OnSuccess_If_Success(Func<StringFaultResult<Guid>, ICounter, Result<StringFault, Guid>> callOnSuccess)
        {
            var counter = Substitute.For<ICounter>();
            var result = StringFaultResult.Succeed(Guid.NewGuid());

            callOnSuccess(result, counter);

            counter.Received().Increment();
        }

        [Test]
        public void Pass_Value_If_Success()
        {
            const string expected = "expected value";
            var result = StringFaultResult.Succeed(expected);
            var consumer = Substitute.For<IConsumer>();

            result.OnSuccess(value => consumer.Consume(value));

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateDoNotCallOnSuccessIfFailureCase(Func<StringFaultResult<Guid>, Result<StringFault, Guid>> assertOnSuccessIsNotCalled)
        {
            return new(assertOnSuccessIsNotCalled);
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly TestCaseData[] DoNotCallOnSuccessIfFailureCases =
        {
            CreateDoNotCallOnSuccessIfFailureCase(result => result.OnSuccess(EnsureOnSuccessIsNotCalled)),
            CreateDoNotCallOnSuccessIfFailureCase(result => result.OnSuccess(_ => EnsureOnSuccessIsNotCalled())),
        };

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public void Do_Not_Call_OnSuccess_If_Failure(Func<StringFaultResult<Guid>, Result<StringFault, Guid>> assertOnSuccessIsNotCalled)
        {
            var result = StringFaultResult.Fail<Guid>(new("failure"));

            assertOnSuccessIsNotCalled(result);
        }

        private static readonly Func<StringFaultResult<int>, Result<StringFault, int>>[] OnSuccessMethods =
        {
            result => result.OnSuccess(_ => { }),
            result => result.OnSuccess(() => { }),
        };

        private static readonly StringFaultResult<int>[] ResultExamples =
        {
            StringFaultResult.Succeed(123),
            StringFaultResult.Fail<int>(new("failure")),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
                from result in ResultExamples
                from method in OnSuccessMethods
                select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<StringFault, int> Return_Self(StringFaultResult<int> result, Func<StringFaultResult<int>, Result<StringFault, int>> callOnSuccess)
        {
            return callOnSuccess(result);
        }
    }
}