using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.TValue
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Action<Result<string, int>> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(result => result.EnsureSuccess(new MyException())),
            CreateCase(result => result.EnsureSuccess(() => new MyException())),
            CreateCase(result => result.EnsureSuccess(_ => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Failure(Action<Result<string, int>> extractor)
        {
            var result = Result<string, int>.Fail("err");

            Action action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Fault_To_Exception_Factory()
        {
            const string expected = "example";
            var result = Result<string, int>.Fail(expected);

            Action action = () => result.EnsureSuccess(fault => new(fault));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.EnsureSuccess()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Do_Not_Throw_If_Success(Action<Result<string, int>> extractor)
        {
            const int expected = 5;
            var result = Result<string, int>.Succeed(expected);

            Action action = () => extractor(result);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Action<Result<int, string>> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.EnsureSuccess(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.EnsureSuccess(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Success(Action<Result<int, string>> assertExtracted)
        {
            var result = Result<int, string>.Succeed("result");

            assertExtracted(result);
        }

        private class MyException : Exception
        {
        }
    }
}
