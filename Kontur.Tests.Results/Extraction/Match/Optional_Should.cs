using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Match
{
    [TestFixture]
    internal class Optional_Should
    {
        private static IEnumerable<Func<Optional<int>, string>> CreateAllMatchMethods(string onNoneValue, string onSomeValue)
        {
            yield return option => option.Match(() => onNoneValue, _ => onSomeValue);
            yield return option => option.Match(() => onNoneValue, () => onSomeValue);
            yield return option => option.Match(() => onNoneValue, onSomeValue);
            yield return option => option.Match(onNoneValue, _ => onSomeValue);
            yield return option => option.Match(onNoneValue, () => onSomeValue);
            yield return option => option.Match(onNoneValue, onSomeValue);
        }

        private static IEnumerable<TestCaseData> ReturnIfSomeCases
        {
            get
            {
                const string expected = "some-value";

                return CreateAllMatchMethods("unreachable", expected)
                    .Select(method => new TestCaseData(method).Returns(expected));
            }
        }

        [TestCaseSource(nameof(ReturnIfSomeCases))]
        public string Return_If_Some(Func<Optional<int>, string> callMatch)
        {
            var option = Optional<int>.Some(0);

            return callMatch(option);
        }

        private static IEnumerable<TestCaseData> ReturnIfNoneCases
        {
            get
            {
                const string expected = "none-value";

                return CreateAllMatchMethods(expected, "unreachable")
                    .Select(method => new TestCaseData(method).Returns(expected));
            }
        }

        [TestCaseSource(nameof(ReturnIfNoneCases))]
        public string Return_If_None(Func<Optional<int>, string> callMatch)
        {
            var option = Optional<int>.None();

            return callMatch(option);
        }

        private static TestCaseData CreateUseValueCase(Func<Optional<int>, string> extractor)
        {
            return new(extractor);
        }

        private static IEnumerable<TestCaseData> UseValueCases
        {
            get
            {
                yield return CreateUseValueCase(option => option.Match(
                    "unused",
                    i => i.ToString(CultureInfo.InvariantCulture)));

                yield return CreateUseValueCase(option => option.Match(
                    () => "unused",
                    i => i.ToString(CultureInfo.InvariantCulture)));
            }
        }

        [TestCaseSource(nameof(UseValueCases))]
        public void Use_Value(Func<Optional<int>, string> extractor)
        {
            var option = Optional<int>.Some(777);

            var result = extractor(option);

            result.Should().Be("777");
        }

        [AssertionMethod]
        private static string AssertIsNotCalled(string branch)
        {
            Assert.Fail(branch + " is called");
            throw new UnreachableException();
        }

        private static string AssertSomeIsNotCalled()
        {
            return AssertIsNotCalled("OnSome");
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Optional<string>, string> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallSomeFactoryIfNoneCases =
        {
            CreateDoNoCallFactoryCase(option => option.Match(() => "unused", _ => AssertSomeIsNotCalled())),
            CreateDoNoCallFactoryCase(option => option.Match(() => "unused", AssertSomeIsNotCalled)),
            CreateDoNoCallFactoryCase(option => option.Match("unused", _ => AssertSomeIsNotCalled())),
            CreateDoNoCallFactoryCase(option => option.Match("unused", AssertSomeIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallSomeFactoryIfNoneCases))]
        public void Do_Not_Call_OnSome_Factory_If_None(Func<Optional<string>, string> assertExtracted)
        {
            var option = Optional<string>.None();

            assertExtracted(option);
        }

        private static string AssertNoneIsNotCalled()
        {
            return AssertIsNotCalled("OnNone");
        }

        private static readonly TestCaseData[] CreateDoNoCallNoneFactoryIfSomeCases =
        {
            CreateDoNoCallFactoryCase(option => option.Match(AssertNoneIsNotCalled, _ => "unused")),
            CreateDoNoCallFactoryCase(option => option.Match(AssertNoneIsNotCalled,  () => "unused")),
            CreateDoNoCallFactoryCase(option => option.Match(AssertNoneIsNotCalled,  "unused")),
        };

        [TestCaseSource(nameof(CreateDoNoCallNoneFactoryIfSomeCases))]
        public void Do_Not_Call_OnNone_If_Some(Func<Optional<string>, string> assertExtracted)
        {
            var option = Optional<string>.Some("foo");

            assertExtracted(option);
        }
    }
}
