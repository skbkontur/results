using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Actions.OnFailure
{
    [TestFixture]
    public class OnNone_Should
    {
        [Test]
        public void Call_OnNone_If_None()
        {
            var counter = Substitute.For<ICounter>();
            var option = Optional<string>.None();

            option.OnNone(() => counter.Increment());

            counter.Received().Increment();
        }

        [Test]
        public void Do_Not_Call_OnNone_If_Some()
        {
            var option = Optional<string>.Some("foo");

            option.OnNone(() => Assert.Fail("OnNone is called"));
        }

        private static TestCaseData CreateReturnSelfCase(Optional<int> optional)
        {
            return new(optional) { ExpectedResult = optional };
        }

        private static readonly TestCaseData[] ReturnSelfCases =
        {
            CreateReturnSelfCase(Optional<int>.None()),
            CreateReturnSelfCase(Optional<int>.Some(1)),
        };

        [TestCaseSource(nameof(ReturnSelfCases))]
        public Optional<int> Return_Self_OnNone(Optional<int> optional)
        {
            return optional.OnNone(() => { });
        }

        private static readonly Func<Optional<Child>, Optional<Base>>[] UpcastMethods =
        {
            option => option.OnNone<Base>(() => { }),
        };

        private static readonly IEnumerable<TestCaseData> UpcastCases =
            from testCase in UpcastOptionalExamples.Get()
            from method in UpcastMethods
            select new TestCaseData(testCase.Optional, method).Returns(testCase.Result);

        [TestCaseSource(nameof(UpcastCases))]
        public Optional<Base> Return_Self_On_Upcast(
            Optional<Child> optional,
            Func<Optional<Child>, Optional<Base>> callOnNone)
        {
            return callOnNone(optional);
        }
    }
}