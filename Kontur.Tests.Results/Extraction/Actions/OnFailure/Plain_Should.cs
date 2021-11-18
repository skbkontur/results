using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnFailure
{
    [TestFixture]
    public class Plain_Should
    {
        private static TestCaseData CreateCallOnFailureIfFailureCase(Func<Result<string>, ICounter, Result<string>> callOnFailure)
        {
            return new(callOnFailure);
        }

        private static readonly TestCaseData[] CallOnFailureIfFailureCases =
        {
            CreateCallOnFailureIfFailureCase((result, counter) => result.OnFailure(counter.Increment)),
            CreateCallOnFailureIfFailureCase((result, counter) => result.OnFailure(_ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public void Call_OnFailure_If_Failure(Func<Result<string>, ICounter, Result<string>> callOnFailure)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<string>.Fail("foo");

            callOnFailure(result, counter);

            counter.Received().Increment();
        }

        [Test]
        public void Pass_Fault_If_Failure()
        {
            const string expected = "foo";
            var result = Result<string>.Fail(expected);
            var consumer = Substitute.For<IConsumer>();

            result.OnFailure(value => consumer.Consume(value));

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateDoNotCallOnFailureIfSuccessCase(Func<Result<string>, Result<string>> assertOnFailureIsNotCalled)
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
        public void Do_Not_Call_OnFailure_If_Success(Func<Result<string>, Result<string>> assertOnFailureIsNotCalled)
        {
            var result = Result<string>.Succeed();

            assertOnFailureIsNotCalled(result);
        }

        private static readonly Func<Result<int>, Result<int>>[] OnFailureMethods =
        {
            result => result.OnFailure(_ => { }),
            result => result.OnFailure(() => { }),
        };

        private static readonly Result<int>[] ResultExamples =
        {
            Result<int>.Fail(123),
            Result<int>.Succeed(),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
                from result in ResultExamples
                from method in OnFailureMethods
                select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<int> Return_Self(Result<int> result, Func<Result<int>, Result<int>> callOnFailure)
        {
            return callOnFailure(result);
        }

        private static readonly Func<Result<Child>, Result<Base>>[] UpcastMethods =
        {
            result => result.OnFailure<Base>(_ => { }),
            result => result.OnFailure<Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastPlainExamples.Get()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<Base> Return_Self_On_Upcast(
            Result<Child> source,
            Func<Result<Child>, Result<Base>> callOnFailure)
        {
            return callOnFailure(source);
        }
    }
}