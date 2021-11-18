using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.Switch
{
    [TestFixture]
    public class Optional_Should
    {
        private static TestCaseData CreateCallSwitchWithCounterCase(Func<Optional<string>, ICounter, Optional<string>> callSwitch)
        {
            return new(callSwitch);
        }

        private static readonly TestCaseData[] CallOnNoneIfNoneCases =
        {
            CreateCallSwitchWithCounterCase((option, counter) => option.Switch(counter.Increment, () => { })),
            CreateCallSwitchWithCounterCase((option, counter) => option.Switch(counter.Increment, _ => { })),
        };

        [TestCaseSource(nameof(CallOnNoneIfNoneCases))]
        public void Call_OnNone_If_None(Func<Optional<string>, ICounter, Optional<string>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var option = Optional<string>.None();

            callSwitch(option, counter);

            counter.Received().Increment();
        }

        private static readonly TestCaseData[] CallOnSomeIfSomeCases =
        {
            CreateCallSwitchWithCounterCase((option, counter) => option.Switch(() => { }, counter.Increment)),
            CreateCallSwitchWithCounterCase((option, counter) => option.Switch(() => { }, _ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnSomeIfSomeCases))]
        public void Call_OnSome_If_Some(Func<Optional<string>, ICounter, Optional<string>> callSwitch)
        {
            var counter = Substitute.For<ICounter>();
            var option = Optional<string>.Some("foo");

            callSwitch(option, counter);

            counter.Received().Increment();
        }

        private static TestCaseData CreateCallSwitchCase<TValue>(Func<Optional<TValue>, Optional<TValue>> callSwitch)
        {
            return new(callSwitch);
        }

        private static TestCaseData CreateDoNotCallOnSomeIfNoneCase(Func<Optional<string>, Optional<string>> assertOnSomeIsNotCalled)
        {
            return CreateCallSwitchCase(assertOnSomeIsNotCalled);
        }

        private static void EnsureOnSomeIsNotCalled()
        {
            Assert.Fail("OnSome is called");
        }

        private static readonly TestCaseData[] DoNotCallOnSomeIfNoneCases =
        {
            CreateDoNotCallOnSomeIfNoneCase(option => option.Switch(() => { }, EnsureOnSomeIsNotCalled)),
            CreateDoNotCallOnSomeIfNoneCase(option => option.Switch(() => { }, _ => EnsureOnSomeIsNotCalled())),
        };

        [TestCaseSource(nameof(DoNotCallOnSomeIfNoneCases))]
        public void Do_Not_Call_OnSome_If_None(Func<Optional<string>, Optional<string>> assertOnSomeIsNotCalled)
        {
            var option = Optional<string>.None();

            assertOnSomeIsNotCalled(option);
        }

        private static TestCaseData CreateDoNotCallOnNoneIfSomeCase(Func<Optional<string>, Optional<string>> assertOnNoneIsNotCalled)
        {
            return CreateCallSwitchCase(assertOnNoneIsNotCalled);
        }

        private static void EnsureOnNoneIsNotCalled()
        {
            Assert.Fail("OnNone is called");
        }

        private static readonly TestCaseData[] DoNotCallOnNoneIfSomeCases =
        {
            CreateDoNotCallOnNoneIfSomeCase(option => option.Switch(EnsureOnNoneIsNotCalled, _ => { })),
            CreateDoNotCallOnNoneIfSomeCase(option => option.Switch(EnsureOnNoneIsNotCalled, () => { })),
        };

        [TestCaseSource(nameof(DoNotCallOnNoneIfSomeCases))]
        public void Do_Not_Call_OnNone_If_Some(Func<Optional<string>, Optional<string>> assertOnNoneIsNotCalled)
        {
            var option = Optional<string>.Some("foo");

            assertOnNoneIsNotCalled(option);
        }

        private static readonly Func<Optional<int>, Optional<int>>[] SwitchMethods =
        {
            option => option.Switch(() => { }, _ => { }),
            option => option.Switch(() => { }, () => { }),
        };

        private static readonly Optional<int>[] OptionExamples =
        {
            Optional<int>.Some(123),
            Optional<int>.None(),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
            from option in OptionExamples
            from method in SwitchMethods
            select new TestCaseData(option, method).Returns(option);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Optional<int> Return_Self(Optional<int> optional, Func<Optional<int>, Optional<int>> callSwitch)
        {
            return callSwitch(optional);
        }

        [Test]
        public void Pass_Value_OnSome()
        {
            const string expected = "foo";
            var option = Optional<string>.Some(expected);
            var consumer = Substitute.For<IConsumer>();

            option.Switch(() => { }, value => consumer.Consume(value));

            consumer.Received().Consume(expected);
        }

        private static readonly Func<Optional<Child>, Optional<Base>>[] UpcastMethods =
        {
            option => option.Switch<Base>(() => { }, _ => { }),
            option => option.Switch<Base>(() => { }, () => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastOptionalExamples.Get()
            from method in UpcastMethods
            select new TestCaseData(testCase.Optional, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Optional<Base> Return_Self_On_Upcast(
            Optional<Child> optional,
            Func<Optional<Child>, Optional<Base>> callSwitch)
        {
            return callSwitch(optional);
        }
    }
}