using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.HasValue
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
            CreateCase(option => option.EnsureHasValue(new MyException())),
            CreateCase(option => option.EnsureHasValue(() => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_None(Action<Optional<int>> extractor)
        {
            var option = Optional<int>.None();

            Action action = () => extractor(option);

            action.Should().Throw<MyException>();
        }

        private static readonly IEnumerable<TestCaseData> SomeCases = MyExceptionCases
            .Append(CreateCase(option => option.EnsureHasValue()));

        [TestCaseSource(nameof(SomeCases))]
        public void Do_Not_Throw_If_Some(Action<Optional<int>> extractor)
        {
            var option = Optional<int>.Some(5);

            Action action = () => extractor(option);

            action.Should().NotThrow();
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on some");
            throw new UnreachableException();
        }

        [Test]
        public void Do_No_Create_Exception_If_Some()
        {
            var option = Optional<int>.Some(5);

            option.EnsureHasValue(AssertIsNotCalled);
        }

        private class MyException : Exception
        {
        }
    }
}
