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
    internal class TValue_Should
    {
        private static IEnumerable<Func<Result<Guid, int>, string>> CreateAllMatchMethods(string onFailureValue, string onSuccessValue)
        {
            yield return result => result.Match(_ => onFailureValue, _ => onSuccessValue);
            yield return result => result.Match(_ => onFailureValue, () => onSuccessValue);
            yield return result => result.Match(_ => onFailureValue, onSuccessValue);
            yield return result => result.Match(() => onFailureValue, _ => onSuccessValue);
            yield return result => result.Match(() => onFailureValue, () => onSuccessValue);
            yield return result => result.Match(() => onFailureValue, onSuccessValue);
            yield return result => result.Match(onFailureValue, _ => onSuccessValue);
            yield return result => result.Match(onFailureValue, () => onSuccessValue);
            yield return result => result.Match(onFailureValue, onSuccessValue);
        }

        private static IEnumerable<TestCaseData> ReturnIfSuccessCases
        {
            get
            {
                const string expected = "success-value";

                return CreateAllMatchMethods("unreachable", expected)
                    .Select(method => new TestCaseData(method).Returns(expected));
            }
        }

        [TestCaseSource(nameof(ReturnIfSuccessCases))]
        public string Return_If_Success(Func<Result<Guid, int>, string> callMatch)
        {
            var result = Result<Guid, int>.Succeed(0);

            return callMatch(result);
        }

        private static IEnumerable<TestCaseData> ReturnIfFailureCases
        {
            get
            {
                const string expected = "fail-value";

                return CreateAllMatchMethods(expected, "unreachable")
                    .Select(method => new TestCaseData(method).Returns(expected));
            }
        }

        [TestCaseSource(nameof(ReturnIfFailureCases))]
        public string Return_If_Failure(Func<Result<Guid, int>, string> callMatch)
        {
            var result = Result<Guid, int>.Fail(Guid.NewGuid());

            return callMatch(result);
        }

        private static TestCaseData CreateUseValueCase(Func<Result<Guid, int>, string> extractor)
        {
            return new(extractor);
        }

        private static IEnumerable<TestCaseData> UseValueCases
        {
            get
            {
                yield return CreateUseValueCase(result => result.Match(
                    "unused",
                    i => i.ToString(CultureInfo.InvariantCulture)));

                yield return CreateUseValueCase(result => result.Match(
                    () => "unused",
                    i => i.ToString(CultureInfo.InvariantCulture)));

                yield return CreateUseValueCase(result => result.Match(
                    _ => "unused",
                    i => i.ToString(CultureInfo.InvariantCulture)));
            }
        }

        [TestCaseSource(nameof(UseValueCases))]
        public void Use_Value(Func<Result<Guid, int>, string> extractor)
        {
            var source = Result<Guid, int>.Succeed(777);

            var result = extractor(source);

            result.Should().Be("777");
        }

        private static TestCaseData CreateUseFaultCase(Func<Result<int, Guid>, string> extractor)
        {
            return new(extractor);
        }

        private static IEnumerable<TestCaseData> UseFaultCases
        {
            get
            {
                yield return CreateUseFaultCase(result => result.Match(
                    i => i.ToString(CultureInfo.InvariantCulture),
                    "unused"));

                yield return CreateUseFaultCase(result => result.Match(
                    i => i.ToString(CultureInfo.InvariantCulture),
                    () => "unused"));

                yield return CreateUseFaultCase(result => result.Match(
                    i => i.ToString(CultureInfo.InvariantCulture),
                    _ => "unused"));
            }
        }

        [TestCaseSource(nameof(UseFaultCases))]
        public void Use_Fault(Func<Result<int, Guid>, string> extractor)
        {
            var source = Result<int, Guid>.Fail(1000);

            var result = extractor(source);

            result.Should().Be("1000");
        }

        [AssertionMethod]
        private static string AssertIsNotCalled(string branch)
        {
            Assert.Fail(branch + " is called");
            throw new UnreachableException();
        }

        private static string AssertSuccessIsNotCalled()
        {
            return AssertIsNotCalled("OnSuccess");
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<Guid, int>, string> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallSuccessFactoryIfFailureCases =
        {
            CreateDoNoCallFactoryCase(result => result.Match(_ => "unused", _ => AssertSuccessIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.Match(_ => "unused", AssertSuccessIsNotCalled)),
            CreateDoNoCallFactoryCase(result => result.Match(() => "unused", _ => AssertSuccessIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.Match(() => "unused", AssertSuccessIsNotCalled)),
            CreateDoNoCallFactoryCase(result => result.Match("unused", _ => AssertSuccessIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.Match("unused", AssertSuccessIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallSuccessFactoryIfFailureCases))]
        public void Do_Not_Call_OnSuccess_Factory_If_Failure(Func<Result<Guid, int>, string> assertExtracted)
        {
            var result = Result<Guid, int>.Fail(Guid.NewGuid());

            assertExtracted(result);
        }

        private static string AssertFailureIsNotCalled()
        {
            return AssertIsNotCalled("OnFailure");
        }

        private static readonly TestCaseData[] CreateDoNoCallFailureFactoryIfSuccessCases =
        {
            CreateDoNoCallFactoryCase(result => result.Match(AssertFailureIsNotCalled, _ => "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(AssertFailureIsNotCalled,  () => "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(AssertFailureIsNotCalled,  "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(_ => AssertFailureIsNotCalled(), _ => "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(_ => AssertFailureIsNotCalled(),  () => "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(_ => AssertFailureIsNotCalled(),  "unused")),
        };

        [TestCaseSource(nameof(CreateDoNoCallFailureFactoryIfSuccessCases))]
        public void Do_Not_Call_OnFailure_If_Success(Func<Result<Guid, int>, string> assertExtracted)
        {
            var result = Result<Guid, int>.Succeed(2);

            assertExtracted(result);
        }
    }
}
