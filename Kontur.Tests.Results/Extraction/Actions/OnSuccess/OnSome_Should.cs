using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnSuccess
{
    [TestFixture]
    public class OnSome_Should
    {
        private static TestCaseData CreateCallOnSomeIfSomeCase(Func<Optional<string>, ICounter, Optional<string>> callOnSome)
        {
            return new(callOnSome);
        }

        private static readonly TestCaseData[] CallOnSomeIfSomeCases =
        {
            CreateCallOnSomeIfSomeCase((option, counter) => option.OnSome(counter.Increment)),
            CreateCallOnSomeIfSomeCase((option, counter) => option.OnSome(_ => counter.Increment())),
        };

        [TestCaseSource(nameof(CallOnSomeIfSomeCases))]
        public void Call_OnSome_If_Some(Func<Optional<string>, ICounter, Optional<string>> callOnSome)
        {
            var counter = Substitute.For<ICounter>();
            var option = Optional<string>.Some("foo");

            callOnSome(option, counter);

            counter.Received().Increment();
        }

        [Test]
        public void Pass_Value_If_Some()
        {
            const string expected = "foo";
            var option = Optional<string>.Some(expected);
            var consumer = Substitute.For<IConsumer>();

            option.OnSome(value => consumer.Consume(value));

            consumer.Received().Consume(expected);
        }

        private static TestCaseData CreateDoNotCallOnSomeIfNoneCase(Func<Optional<string>, Optional<string>> assertOnSomeIsNotCalled)
        {
            return new(assertOnSomeIsNotCalled);
        }

        private static void EnsureOnSomeIsNotCalled()
        {
            Assert.Fail("OnSome is called");
        }

        private static readonly TestCaseData[] DoNotCallOnSomeIfNoneCases =
        {
            CreateDoNotCallOnSomeIfNoneCase(option => option.OnSome(EnsureOnSomeIsNotCalled)),
            CreateDoNotCallOnSomeIfNoneCase(option => option.OnSome(_ => EnsureOnSomeIsNotCalled())),
        };

        [TestCaseSource(nameof(DoNotCallOnSomeIfNoneCases))]
        public void Do_Not_Call_OnSome_If_None(Func<Optional<string>, Optional<string>> assertOnSomeIsNotCalled)
        {
            var option = Optional<string>.None();

            assertOnSomeIsNotCalled(option);
        }

        private static readonly Func<Optional<int>, Optional<int>>[] OnSomeMethods =
        {
            option => option.OnSome(_ => { }),
            option => option.OnSome(() => { }),
        };

        private static readonly Optional<int>[] OptionExamples =
        {
            Optional<int>.Some(123),
            Optional<int>.None(),
        };

        private static readonly IEnumerable<TestCaseData> ReturnSelfCases =
                from option in OptionExamples
                from method in OnSomeMethods
                select new TestCaseData(option, method).Returns(option);

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Optional<int> Return_Self(Optional<int> optional, Func<Optional<int>, Optional<int>> callOnSome)
        {
            return callOnSome(optional);
        }

        private static readonly Func<Optional<Child>, Optional<Base>>[] UpcastMethods =
        {
            option => option.OnSome<Base>(_ => { }),
            option => option.OnSome<Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastOptionalExamples.Get()
            from method in UpcastMethods
            select new TestCaseData(testCase.Optional, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Optional<Base> Return_Self_On_Upcast(
            Optional<Child> optional,
            Func<Optional<Child>, Optional<Base>> callOnSome)
        {
            return callOnSome(optional);
        }
    }
}