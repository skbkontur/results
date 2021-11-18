using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.Plain
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Action<Result<int>> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(result => result.EnsureFailure(new MyException())),
            CreateCase(result => result.EnsureFailure(() => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Success(Action<Result<int>> extractor)
        {
            var result = Result<int>.Succeed();

            Action action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        private static readonly IEnumerable<TestCaseData> FailureCases = MyExceptionCases
            .Append(CreateCase(result => result.EnsureFailure()));

        [TestCaseSource(nameof(FailureCases))]
        public void Do_Not_Throw_If_Failure(Action<Result<int>> extractor)
        {
            const int expected = 5;
            var result = Result<int>.Fail(expected);

            Action action = () => extractor(result);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        [Test]
        public void Do_No_Create_Exception_If_Success()
        {
            var result = Result<int>.Fail(5);

            result.EnsureFailure(AssertIsNotCalled);
        }

        private class MyException : Exception
        {
        }
    }
}
