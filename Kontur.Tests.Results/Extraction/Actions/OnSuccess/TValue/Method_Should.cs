using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnSuccess.TValue
{
    [TestFixture]
    public class Method_Should
    {
        private static TestCaseData CreateCallOnSuccessIfSuccessCase(Func<Result<Guid, string>, ICounter, Result<Guid, string>> callOnSuccess)
        {
            return new(callOnSuccess);
        }

        private static readonly TestCaseData[] CallOnSuccessIfSuccessCases =
        {
            CreateCallOnSuccessIfSuccessCase((result, counter) => result.OnSuccess(counter.Increment)),
            CreateCallOnSuccessIfSuccessCase((result, counter) => result.OnSuccess(_ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public void Call_OnSuccess_If_Success(Func<Result<Guid, string>, ICounter, Result<Guid, string>> callOnSuccess)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<Guid, string>.Succeed("foo");

            callOnSuccess(result, counter);

            counter.Received().Increment();
        }

        [Test]
        public void Pass_Value_If_Success()
        {
            const string expected = "foo";
            var result = Result<int, string>.Succeed(expected);
            var consumer = Substitute.For<IConsumer>();

            result.OnSuccess(value => consumer.Consume(value));

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateDoNotCallOnSuccessIfFailureCase(Func<Result<Guid, string>, Result<Guid, string>> assertOnSuccessIsNotCalled)
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
        public void Do_Not_Call_OnSuccess_If_Failure(Func<Result<Guid, string>, Result<Guid, string>> assertOnSuccessIsNotCalled)
        {
            var result = Result<Guid, string>.Fail(Guid.NewGuid());

            assertOnSuccessIsNotCalled(result);
        }

        private static readonly Func<Result<Guid, int>, Result<Guid, int>>[] OnSuccessMethods =
        {
            result => result.OnSuccess(_ => { }),
            result => result.OnSuccess(() => { }),
        };

        private static readonly Result<Guid, int>[] ResultExamples =
        {
            Result<Guid, int>.Succeed(123),
            Result<Guid, int>.Fail(Guid.NewGuid()),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
                from result in ResultExamples
                from method in OnSuccessMethods
                select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<Guid, int> Return_Self(Result<Guid, int> result, Func<Result<Guid, int>, Result<Guid, int>> callOnSuccess)
        {
            return callOnSuccess(result);
        }
    }
}