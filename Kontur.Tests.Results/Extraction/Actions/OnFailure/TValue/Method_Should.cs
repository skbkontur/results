using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnFailure.TValue
{
    [TestFixture]
    public class Method_Should
    {
        private static TestCaseData CreateCallOnFailureIfFailureCase(Func<Result<string, Guid>, ICounter, Result<string, Guid>> callOnFailure)
        {
            return new(callOnFailure);
        }

        private static readonly TestCaseData[] CallOnFailureIfFailureCases =
        {
            CreateCallOnFailureIfFailureCase((result, counter) => result.OnFailure(counter.Increment)),
            CreateCallOnFailureIfFailureCase((result, counter) => result.OnFailure(_ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public void Call_OnFailure_If_Failure(Func<Result<string, Guid>, ICounter, Result<string, Guid>> callOnFailure)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<string, Guid>.Fail("foo");

            callOnFailure(result, counter);

            counter.Received().Increment();
        }

        [Test]
        public void Pass_Fault_If_Failure()
        {
            const string expected = "foo";
            var result = Result<string, Guid>.Fail(expected);
            var consumer = Substitute.For<IConsumer>();

            result.OnFailure(fault => consumer.Consume(fault));

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateDoNotCallOnFailureIfSuccessCase(Func<Result<string, Guid>, Result<string, Guid>> assertOnFailureIsNotCalled)
        {
            return new(assertOnFailureIsNotCalled);
        }

        private static void EnsureOnFailureIsNotCalled()
        {
            Assert.Fail("OnFailure is called");
        }

        private static readonly TestCaseData[] DoNotCallOnFailureIfSuccessCases =
        {
            CreateDoNotCallOnFailureIfSuccessCase(result => result.OnFailure(EnsureOnFailureIsNotCalled)),
            CreateDoNotCallOnFailureIfSuccessCase(result => result.OnFailure(_ => EnsureOnFailureIsNotCalled())),
        };

        [TestCaseSource(nameof(DoNotCallOnFailureIfSuccessCases))]
        public void Do_Not_Call_OnFailure_If_Success(Func<Result<string, Guid>, Result<string, Guid>> assertOnFailureIsNotCalled)
        {
            var result = Result<string, Guid>.Succeed(Guid.NewGuid());

            assertOnFailureIsNotCalled(result);
        }

        private static readonly Func<Result<int, Guid>, Result<int, Guid>>[] OnFailureMethods =
        {
            result => result.OnFailure(_ => { }),
            result => result.OnFailure(() => { }),
        };

        private static readonly Result<int, Guid>[] ResultExamples =
        {
            Result<int, Guid>.Fail(123),
            Result<int, Guid>.Succeed(Guid.NewGuid()),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
                from result in ResultExamples
                from method in OnFailureMethods
                select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<int, Guid> Return_Self(Result<int, Guid> result, Func<Result<int, Guid>, Result<int, Guid>> callOnFailure)
        {
            return callOnFailure(result);
        }
    }
}