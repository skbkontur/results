using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.OnFailure
{
    [TestFixture]
    public class OnFailure_Should
    {
        private static TestCaseData CreateCallOnFailureIfFailureCase(Func<StringFaultResult<Guid>, ICounter, Result<StringFault, Guid>> callOnFailure)
        {
            return new(callOnFailure);
        }

        private static readonly TestCaseData[] CallOnFailureIfFailureCases =
        {
            CreateCallOnFailureIfFailureCase((result, counter) => result.OnFailure(counter.Increment)),
            CreateCallOnFailureIfFailureCase((result, counter) => result.OnFailure(_ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public void Call_OnFailure_If_Failure(Func<StringFaultResult<Guid>, ICounter, Result<StringFault, Guid>> callOnFailure)
        {
            var counter = Substitute.For<ICounter>();
            var result = StringFaultResult.Fail<Guid>(new("foo"));

            callOnFailure(result, counter);

            counter.Received().Increment();
        }

        [Test]
        public void Pass_Fault_If_Failure()
        {
            StringFault expected = new("foo");
            var result = StringFaultResult.Fail<Guid>(expected);
            var consumer = Substitute.For<IStringFaultConsumer>();

            result.OnFailure(fault => consumer.Consume(fault));

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateDoNotCallOnFailureIfSuccessCase(Func<StringFaultResult<Guid>, Result<StringFault, Guid>> assertOnFailureIsNotCalled)
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
        public void Do_Not_Call_OnFailure_If_Success(Func<StringFaultResult<Guid>, Result<StringFault, Guid>> assertOnFailureIsNotCalled)
        {
            var result = StringFaultResult.Succeed(Guid.NewGuid());

            assertOnFailureIsNotCalled(result);
        }

        private static readonly Func<StringFaultResult<Guid>, Result<StringFault, Guid>>[] OnFailureMethods =
        {
            result => result.OnFailure(_ => { }),
            result => result.OnFailure(() => { }),
        };

        private static readonly StringFaultResult<Guid>[] ResultExamples =
        {
            StringFaultResult.Fail<Guid>(new("123")),
            StringFaultResult.Succeed(Guid.NewGuid()),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
                from result in ResultExamples
                from method in OnFailureMethods
                select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<StringFault, Guid> Return_Self(
            StringFaultResult<Guid> result,
            Func<StringFaultResult<Guid>, Result<StringFault, Guid>> callOnFailure)
        {
            return callOnFailure(result);
        }
    }
}