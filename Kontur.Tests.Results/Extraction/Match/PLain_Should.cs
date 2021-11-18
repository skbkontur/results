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
    internal class PLain_Should
    {
        private static IEnumerable<Func<Result<int>, string>> CreateAllMatchMethods(string onSuccessValue, string onFailureValue)
        {
            yield return result => result.Match(_ => onFailureValue, () => onSuccessValue);
            yield return result => result.Match(() => onFailureValue, () => onSuccessValue);
            yield return result => result.Match(onFailureValue, () => onSuccessValue);
            yield return result => result.Match(_ => onFailureValue, onSuccessValue);
            yield return result => result.Match(onFailureValue, () => onSuccessValue);
            yield return result => result.Match(onFailureValue, onSuccessValue);
        }

        private static IEnumerable<TestCaseData> ReturnIfFailureCases
        {
            get
            {
                const string expected = "failure-value";

                return CreateAllMatchMethods("unreachable", expected)
                    .Select(method => new TestCaseData(method).Returns(expected));
            }
        }

        [TestCaseSource(nameof(ReturnIfFailureCases))]
        public string Return_If_Failure(Func<Result<int>, string> callMatch)
        {
            var result = Result<int>.Fail(0);

            return callMatch(result);
        }

        private static IEnumerable<TestCaseData> ReturnIfSuccessCases
        {
            get
            {
                const string expected = "success-value";

                return CreateAllMatchMethods(expected, "unreachable")
                    .Select(method => new TestCaseData(method).Returns(expected));
            }
        }

        [TestCaseSource(nameof(ReturnIfSuccessCases))]
        public string Return_If_Success(Func<Result<int>, string> callMatch)
        {
            var succeed = Result<int>.Succeed();

            return callMatch(succeed);
        }

        private static TestCaseData CreateUseFaultCase(Func<Result<int>, string> extractor)
        {
            return new(extractor);
        }

        private static IEnumerable<TestCaseData> UseFaultCases
        {
            get
            {
                yield return CreateUseFaultCase(result => result.Match(i => i.ToString(CultureInfo.InvariantCulture), "unused"));

                yield return CreateUseFaultCase(result => result.Match(i => i.ToString(CultureInfo.InvariantCulture), () => "unused"));
            }
        }

        [TestCaseSource(nameof(UseFaultCases))]
        public void Use_Fault(Func<Result<int>, string> extractor)
        {
            var source = Result<int>.Fail(777);

            var result = extractor(source);

            result.Should().Be("777");
        }

        [AssertionMethod]
        private static string AssertIsNotCalled(string branch)
        {
            Assert.Fail(branch + " is called");
            throw new UnreachableException();
        }

        private static string AssertFailureIsNotCalled()
        {
            return AssertIsNotCalled("OnFailure");
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<string>, string> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFaultFactoryIfSuccessCases =
        {
            CreateDoNoCallFactoryCase(result => result.Match(_ => AssertFailureIsNotCalled(), () => "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(AssertFailureIsNotCalled, () => "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(_ => AssertFailureIsNotCalled(), "unused")),
            CreateDoNoCallFactoryCase(result => result.Match(AssertFailureIsNotCalled, "unused")),
        };

        [TestCaseSource(nameof(CreateDoNoCallFaultFactoryIfSuccessCases))]
        public void Do_Not_Call_OnFailure_Factory_If_Success(Func<Result<string>, string> assertExtracted)
        {
            var result = Result<string>.Succeed();

            assertExtracted(result);
        }

        private static string AssertSuccessIsNotCalled()
        {
            return AssertIsNotCalled("OnSuccess");
        }

        private static readonly TestCaseData[] CreateDoNoCallSuccessFactoryIfFailureCases =
        {
            CreateDoNoCallFactoryCase(result => result.Match(_ => "unused", AssertSuccessIsNotCalled)),
            CreateDoNoCallFactoryCase(result => result.Match(() => "unused", AssertSuccessIsNotCalled)),
            CreateDoNoCallFactoryCase(result => result.Match("unused", AssertSuccessIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallSuccessFactoryIfFailureCases))]
        public void Do_Not_Call_OnSuccess_If_Failure(Func<Result<string>, string> assertExtracted)
        {
            var result = Result<string>.Fail("foo");

            assertExtracted(result);
        }
    }
}
