using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Actions.Switch
{
    [TestFixture]
    public class Switch_Should
    {
        private static TestCaseData CreateCallSwitchWithCounterCase(Func<StringFaultResult<int>, ICounter, Result<StringFault, int>> callSwitch)
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
        public void Call_OnFailure_If_Failure(Func<StringFaultResult<int>, ICounter, Result<StringFault, int>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var result = StringFaultResult.Fail<int>(new("error message"));

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
        public void Call_OnSuccess_If_Success(Func<StringFaultResult<int>, ICounter, Result<StringFault, int>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var result = StringFaultResult.Succeed(50);

            callSwitch(result, counter);

            counter.Received().Increment();
        }

        private static TestCaseData CreateCallSwitchCase<TFault, TValue>(Func<StringFaultResult<TValue>, Result<TFault, TValue>> callSwitch)
        {
            return new(callSwitch);
        }

        private static TestCaseData CreateDoNotCallOnSuccessIfFailureCase(Func<StringFaultResult<Guid>, Result<StringFault, Guid>> assertOnSuccessIsNotCalled)
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
        public void Do_Not_Call_OnSuccess_If_Failure(Func<StringFaultResult<Guid>, Result<StringFault, Guid>> assertOnSuccessIsNotCalled)
        {
            var result = StringFaultResult.Fail<Guid>(new("message"));

            assertOnSuccessIsNotCalled(result);
        }

        private static TestCaseData CreateDoNotCallOnFailureIfSuccessCase(Func<StringFaultResult<int>, Result<StringFault, int>> assertOnFailureIsNotCalled)
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
        public void Do_Not_Call_OnFailure_If_Success(Func<StringFaultResult<int>, Result<StringFault, int>> assertOnFailureIsNotCalled)
        {
            var result = StringFaultResult.Succeed(5);

            assertOnFailureIsNotCalled(result);
        }

        private static readonly Func<StringFaultResult<int>, Result<StringFault, int>>[] SwitchMethods =
        {
            result => result.Switch(() => { }, _ => { }),
            result => result.Switch(() => { }, () => { }),
            result => result.Switch(_ => { }, _ => { }),
            result => result.Switch(_ => { }, () => { }),
        };

        private static readonly StringFaultResult<int>[] ResultExamples =
        {
            StringFaultResult.Fail<int>(new("error message")),
            StringFaultResult.Succeed(50),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            from result in ResultExamples
            from method in SwitchMethods
            select new TestCaseData(result, method).Returns(result);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Result<StringFault, int> Return_Self(
            StringFaultResult<int> result,
            Func<StringFaultResult<int>, Result<StringFault, int>> callSwitch)
        {
            return callSwitch(result);
        }

        private static TestCaseData CreateCallSwitchWithTConsumerCase<TConsumer>(Func<StringFaultResult<string>, TConsumer, Result<StringFault, string>> callSwitch)
        {
            return new(callSwitch);
        }

        private static TestCaseData CreateCallSwitchWithConsumerCase(Func<StringFaultResult<string>, IConsumer, Result<StringFault, string>> callSwitch)
        {
            return CreateCallSwitchWithTConsumerCase(callSwitch);
        }

        private static readonly TestCaseData[] PassValueIfSuccessCases =
        {
            CreateCallSwitchWithConsumerCase((result, consumer) => result.Switch(() => { }, consumer.Consume)),
            CreateCallSwitchWithConsumerCase((result, consumer) => result.Switch(_ => { }, consumer.Consume)),
        };

        [TestCaseSource(nameof(PassValueIfSuccessCases))]
        public void Pass_Value_If_Success(Func<StringFaultResult<string>, IConsumer, Result<StringFault, string>> callSwitch)
        {
            const string expected = "foo";
            var result = StringFaultResult.Succeed(expected);
            var consumer = Substitute.For<IConsumer>();

            callSwitch(result, consumer);

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateCallSwitchWithStringFaultConsumerCase(Func<StringFaultResult<string>, IStringFaultConsumer, Result<StringFault, string>> callSwitch)
        {
            return CreateCallSwitchWithTConsumerCase(callSwitch);
        }

        private static readonly TestCaseData[] PassValueIfFailureCases =
        {
            CreateCallSwitchWithStringFaultConsumerCase((result, consumer) => result.Switch(consumer.Consume, () => { })),
            CreateCallSwitchWithStringFaultConsumerCase((result, consumer) => result.Switch(consumer.Consume, _ => { })),
        };

        [TestCaseSource(nameof(PassValueIfFailureCases))]
        public void Pass_Fault_If_Failure(Func<StringFaultResult<string>, IStringFaultConsumer, Result<StringFault, string>> callSwitch)
        {
            StringFault expected = new("bar");
            var result = StringFaultResult.Fail<string>(expected);
            var consumer = Substitute.For<IStringFaultConsumer>();

            callSwitch(result, consumer);

            consumer.Received().Consume(expected);
        }
    }
}