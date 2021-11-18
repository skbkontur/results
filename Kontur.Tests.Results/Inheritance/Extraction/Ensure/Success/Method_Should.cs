using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Ensure.Success
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Action<StringFaultResult<int>> extractor)
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
        public void Throw_MyException_If_Failure(Action<StringFaultResult<int>> extractor)
        {
            var result = StringFaultResult.Fail<int>(new("err"));

            Action action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Fault_To_Exception_Factory()
        {
            const string expected = "example";
            var result = StringFaultResult.Fail<int>(new(expected));

            Action action = () => result.EnsureSuccess(fault => new(fault.ToString()));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.EnsureSuccess()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Do_Not_Throw_If_Success(Action<StringFaultResult<int>> extractor)
        {
            const int expected = 5;
            var result = StringFaultResult.Succeed(expected);

            Action action = () => extractor(result);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Action<StringFaultResult<int>> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.EnsureSuccess(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.EnsureSuccess(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Success(Action<StringFaultResult<int>> assertExtracted)
        {
            var result = StringFaultResult.Succeed(50);

            assertExtracted(result);
        }

        private class MyException : Exception
        {
        }
    }
}
