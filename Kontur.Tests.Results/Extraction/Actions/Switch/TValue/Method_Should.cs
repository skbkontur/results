using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.Switch.TValue
{
    [TestFixture]
    public class Method_Should
    {
        private static TestCaseData CreateCallSwitchWithCounterCase(Func<Result<int, string>, ICounter, Result<int, string>> callSwitch)
        {
            return new(callSwitch);
        }

        private static readonly TestCaseData[] CallOnFailureIfFailureCases =
        {
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(counter.Increment, () => { })),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(counter.Increment, _ => { })),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(_ => counter.Increment(), () => { })),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(_ => counter.Increment(), _ => { })),
        };

        [TestCaseSource(nameof(CallOnFailureIfFailureCases))]
        public void Call_OnFailure_If_Failure(Func<Result<int, string>, ICounter, Result<int, string>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<int, string>.Fail(20);

            callSwitch(result, counter);

            counter.Received().Increment();
        }

        private static readonly TestCaseData[] CallOnSuccessIfSuccessCases =
        {
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(() => { }, counter.Increment)),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(() => { }, _ => counter.Increment())),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(_ => { }, counter.Increment)),
            CreateCallSwitchWithCounterCase((result, counter) => result.Switch(_ => { }, _ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnSuccessIfSuccessCases))]
        public void Call_OnSuccess_If_Success(Func<Result<int, string>, ICounter, Result<int, string>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var result = Result<int, string>.Succeed("bar");

            callSwitch(result, counter);

            counter.Received().Increment();
        }

        private static TestCaseData CreateCallSwitchCase<TFault, TValue>(Func<Result<TFault, TValue>, Result<TFault, TValue>> callSwitch)
        {
            return new(callSwitch);
        }

        private static TestCaseData CreateDoNotCallOnSuccessIfFailureCase(Func<Result<Guid, string>, Result<Guid, string>> assertOnSuccessIsNotCalled)
        {
            return CreateCallSwitchCase(assertOnSuccessIsNotCalled);
        }

        private static void EnsureOnSuccessIsNotCalled()
        {
            Assert.Fail("OnSuccess is called");
        }

        private static readonly TestCaseData[] DoNotCallOnSuccessIfFailureCases =
        {
            CreateDoNotCallOnSuccessIfFailureCase(result => result.Switch(() => { }, EnsureOnSuccessIsNotCalled)),
            CreateDoNotCallOnSuccessIfFailureCase(result => result.Switch(() => { }, _ => EnsureOnSuccessIsNotCalled())),
            CreateDoNotCallOnSuccessIfFailureCase(result => result.Switch(_ => { }, EnsureOnSuccessIsNotCalled)),
            CreateDoNotCallOnSuccessIfFailureCase(result => result.Switch(_ => { }, _ => EnsureOnSuccessIsNotCalled())),
        };

        [TestCaseSource(nameof(DoNotCallOnSuccessIfFailureCases))]
        public void Do_Not_Call_OnSuccess_If_Failure(Func<Result<Guid, string>, Result<Guid, string>> assertOnSuccessIsNotCalled)
        {
            var result = Result<Guid, string>.Fail(Guid.NewGuid());

            assertOnSuccessIsNotCalled(result);
        }

        private static TestCaseData CreateDoNotCallOnFailureIfSuccessCase(Func<Result<string, int>, Result<string, int>> assertOnFailureIsNotCalled)
        {
            return CreateCallSwitchCase(assertOnFailureIsNotCalled);
        }

        private static void EnsureOnFailureIsNotCalled()
        {
            Assert.Fail("OnFailure is called");
        }

        private static readonly TestCaseData[] DoNotCallOnFailureIfSuccessCases =
        {
            CreateDoNotCallOnFailureIfSuccessCase(result => result.Switch(EnsureOnFailureIsNotCalled, _ => { })),
            CreateDoNotCallOnFailureIfSuccessCase(result => result.Switch(EnsureOnFailureIsNotCalled, () => { })),
            CreateDoNotCallOnFailureIfSuccessCase(result => result.Switch(_ => EnsureOnFailureIsNotCalled(), _ => { })),
            CreateDoNotCallOnFailureIfSuccessCase(result => result.Switch(_ => EnsureOnFailureIsNotCalled(), () => { })),
        };

        [TestCaseSource(nameof(DoNotCallOnFailureIfSuccessCases))]
        public void Do_Not_Call_OnFailure_If_Success(Func<Result<string, int>, Result<string, int>> assertOnFailureIsNotCalled)
        {
            var result = Result<string, int>.Succeed(5);

            assertOnFailureIsNotCalled(result);
        }

        private static readonly Func<Result<int, string>, Result<int, string>>[] SwitchMethods =
        {
            result => result.Switch(() => { }, _ => { }),
            result => result.Switch(() => { }, () => { }),
            result => result.Switch(_ => { }, _ => { }),
            result => result.Switch(_ => { }, () => { }),
        };

        private static readonly Result<int, string>[] ResultExamples =
        {
            Result<int, string>.Fail(3),
            Result<int, string>.Succeed("yes"),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            from result in ResultExamples
            from method in SwitchMethods
            select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<int, string> Return_Self(Result<int, string> result, Func<Result<int, string>, Result<int, string>> callSwitch)
        {
            return callSwitch(result);
        }

        private static TestCaseData CreateCallSwitchWithConsumerCase(Func<Result<string, string>, IConsumer, Result<string, string>> callSwitch)
        {
            return new(callSwitch);
        }

        private static readonly TestCaseData[] PassValueIfSuccessCases =
        {
            CreateCallSwitchWithConsumerCase((result, consumer) => result.Switch(() => { }, consumer.Consume)),
            CreateCallSwitchWithConsumerCase((result, consumer) => result.Switch(_ => { }, consumer.Consume)),
        };

        [TestCaseSource(nameof(PassValueIfSuccessCases))]
        public void Pass_Value_If_Success(Func<Result<string, string>, IConsumer, Result<string, string>> callSwitch)
        {
            const string expected = "foo";
            var result = Result<string, string>.Succeed(expected);
            var consumer = Substitute.For<IConsumer>();

            callSwitch(result, consumer);

            consumer.Received().Consume(expected);
        }

        private static readonly TestCaseData[] PassValueIfFailureCases =
        {
            CreateCallSwitchWithConsumerCase((result, consumer) => result.Switch(consumer.Consume, () => { })),
            CreateCallSwitchWithConsumerCase((result, consumer) => result.Switch(consumer.Consume, _ => { })),
        };

        [TestCaseSource(nameof(PassValueIfFailureCases))]
        public void Pass_Fault_If_Failure(Func<Result<string, string>, IConsumer, Result<string, string>> callSwitch)
        {
            const string expected = "bar";
            var result = Result<string, string>.Fail(expected);
            var consumer = Substitute.For<IConsumer>();

            callSwitch(result, consumer);

            consumer.Received().Consume(expected);
        }
    }
}