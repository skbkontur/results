using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.Switch
{
    [TestFixture]
    public class Plain_Should
    {
        private static TestCaseData CreateCallSwitchWithCounterCase(Func<Result<string>, ICounter, Result<string>> callSwitch)
        {
            return new(callSwitch);
        }

        private static readonly TestCaseData[] CallOnSuccessIfSuccessCases =
        {
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(() => { }, counter.Increment)),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(_ => { }, counter.Increment)),
        };

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public void Call_OnSuccess_If_Success(Func<Result<string>, ICounter, Result<string>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<string>.Succeed();

            callSwitch(result, counter);

            counter.Received().Increment();
        }

        private static readonly TestCaseData[] CallOnFailureIfFailureCases =
        {
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(counter.Increment, () => { })),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(_ => counter.Increment(), () => { })),
        };

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public void Call_OnFailure_If_Failure(Func<Result<string>, ICounter, Result<string>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<string>.Fail("foo");

            callSwitch(result, counter);

            counter.Received().Increment();
        }

        private static TestCaseData CreateCallSwitchCase<TFault>(Func<Result<TFault>, Result<TFault>> callSwitch)
        {
            return new(callSwitch);
        }

        private static TestCaseData CreateDoNotCallOnFailureIfSuccessCase(Func<Result<string>, Result<string>> assertOnFailureIsNotCalled)
        {
            return CreateCallSwitchCase(assertOnFailureIsNotCalled);
        }

        private static void EnsureOnFailureIsNotCalled()
        {
            Assert.Fail("OnFailure is called");
        }

        private static readonly TestCaseData[] DoNotCallOnFailureIfSuccessCases =
        {
            CreateDoNotCallOnFailureIfSuccessCase(result => result.Switch(EnsureOnFailureIsNotCalled, () => { })),
            CreateDoNotCallOnFailureIfSuccessCase(result => result.Switch(_ => EnsureOnFailureIsNotCalled(), () => { })),
        };

        [TestCaseSource(nameof(DoNotCallOnFailureIfSuccessCases))]
        public void Do_Not_Call_OnFailure_If_Success(Func<Result<string>, Result<string>> assertOnFailureIsNotCalled)
        {
            var result = Result<string>.Succeed();

            assertOnFailureIsNotCalled(result);
        }

        private static TestCaseData CreateDoNotCallOnSuccessIfFailureCase(Func<Result<string>, Result<string>> assertOnSuccessIsNotCalled)
        {
            return CreateCallSwitchCase(assertOnSuccessIsNotCalled);
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly TestCaseData[] DoNotCallOnSuccessIfFailureCases =
        {
            CreateDoNotCallOnSuccessIfFailureCase(result => result.Switch(_ => { }, EnsureOnSuccessIsNotCalled)),
            CreateDoNotCallOnSuccessIfFailureCase(result => result.Switch(() => { }, EnsureOnSuccessIsNotCalled)),
        };

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public void Do_Not_Call_OnSuccess_If_Failure(Func<Result<string>, Result<string>> assertOnSuccessIsNotCalled)
        {
            var result = Result<string>.Fail("foo");

            assertOnSuccessIsNotCalled(result);
        }

        private static readonly Func<Result<int>, Result<int>>[] SwitchMethods =
        {
            result => result.Switch(_ => { }, () => { }),
            result => result.Switch(() => { }, () => { }),
        };

        private static readonly Result<int>[] ResultExamples =
        {
            Result<int>.Fail(123),
            Result<int>.Succeed(),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            from result in ResultExamples
            from method in SwitchMethods
            select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<int> Return_Self(Result<int> source, Func<Result<int>, Result<int>> callSwitch)
        {
            return callSwitch(source);
        }

        [Test]
        public void Pass_Fault_OnFailure()
        {
            const string expected = "foo";
            var result = Result<string>.Fail(expected);
            var consumer = Substitute.For<IConsumer>();

            result.Switch(value => consumer.Consume(value), () => { });

            consumer.Received().Consume(expected);
        }

        private static readonly Func<Result<Child>, Result<Base>>[] UpcastMethods =
        {
            result => result.Switch<Base>(_ => { }, () => { }),
            result => result.Switch<Base>(() => { }, () => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastPlainExamples.Get()
            from method in UpcastMethods
            select new TestCaseData(testCase.Source, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Result<Base> Return_Self_On_Upcast(
            Result<Child> source,
            Func<Result<Child>, Result<Base>> callSwitch)
        {
            return callSwitch(source);
        }
    }
}