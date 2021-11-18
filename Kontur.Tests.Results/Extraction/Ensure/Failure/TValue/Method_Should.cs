using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.TValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Action<Result<int, string>> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(result => result.EnsureFailure(new MyException())),
            CreateCase(result => result.EnsureFailure(() => new MyException())),
            CreateCase(result => result.EnsureFailure(_ => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Success(Action<Result<int, string>> extractor)
        {
            var result = Result<int, string>.Succeed("val");

            Action action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Value_To_Exception_Factory()
        {
            const string expected = "example";
            var result = Result<int, string>.Succeed(expected);

            Action action = () => result.EnsureFailure(value => new(value));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.EnsureFailure()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Do_Not_Throw_If_Failure(Action<Result<int, string>> extractor)
        {
            const int expected = 5;
            var result = Result<int, string>.Fail(expected);

            Action action = () => extractor(result);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Action<Result<int, string>> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.EnsureFailure(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.EnsureFailure(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Failure(Action<Result<int, string>> assertExtracted)
        {
            var result = Result<int, string>.Fail(5);

            assertExtracted(result);
        }

        private class MyException : Exception
        {
        }
    }
}
