using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Ensure.Failure
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
            CreateCase(result => result.EnsureFailure(new MyException())),
            CreateCase(result => result.EnsureFailure(() => new MyException())),
            CreateCase(result => result.EnsureFailure(_ => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Success(Action<StringFaultResult<int>> extractor)
        {
            var result = StringFaultResult.Succeed(50);

            Action action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Value_To_Exception_Factory()
        {
            const string expected = "passed";
            var result = StringFaultResult.Succeed(expected);

            Action action = () => result.EnsureFailure(value => new(value));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.EnsureFailure()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Do_Not_Throw_If_Failure(Action<StringFaultResult<int>> extractor)
        {
            StringFault expected = new("bar");
            var result = StringFaultResult.Fail<int>(expected);

            Action action = () => extractor(result);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Action<StringFaultResult<int>> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.EnsureFailure(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.EnsureFailure(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Failure(Action<StringFaultResult<int>> assertExtracted)
        {
            var result = StringFaultResult.Fail<int>(new("fault"));

            assertExtracted(result);
        }

        private class MyException : Exception
        {
        }
    }
}
