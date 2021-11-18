using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.None
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Action<Optional<int>> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(option => option.EnsureNone(new MyException())),
            CreateCase(option => option.EnsureNone(() => new MyException())),
            CreateCase(option => option.EnsureNone(_ => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Some(Action<Optional<int>> extractor)
        {
            var option = Optional<int>.Some(5);

            Action action = () => extractor(option);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Value_To_Exception_Factory()
        {
            const string expected = "example";
            var option = Optional<string>.Some(expected);

            Action action = () => option.EnsureNone(value => new(value));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SomeCases = MyExceptionCases
            .Append(CreateCase(option => option.EnsureNone()));

        [TestCaseSource(nameof(SomeCases))]
        public void Do_Not_Throw_If_None(Action<Optional<int>> extractor)
        {
            var option = Optional<int>.None();

            Action action = () => extractor(option);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on none");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Action<Optional<int>> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(option => option.EnsureNone(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(option => option.EnsureNone(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Failure(Action<Optional<int>> assertExtracted)
        {
            var option = Optional<int>.None();

            assertExtracted(option);
        }

        private class MyException : Exception
        {
        }
    }
}
