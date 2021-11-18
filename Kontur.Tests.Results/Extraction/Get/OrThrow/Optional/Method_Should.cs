using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Optional
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Func<Optional<int>, int> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(option => option.GetValueOrThrow(new MyException())),
            CreateCase(option => option.GetValueOrThrow(() => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_None(Func<Optional<int>, int> extractor)
        {
            var option = Optional<int>.None();

            Func<int> action = () => extractor(option);

            action.Should().Throw<MyException>();
        }

        private static readonly IEnumerable<TestCaseData> SomeCases = MyExceptionCases
            .Append(CreateCase(option => option.GetValueOrThrow()));

        [TestCaseSource(nameof(SomeCases))]
        public void Return_Value_If_Some(Func<Optional<int>, int> extractor)
        {
            const int expected = 5;
            var option = Optional<int>.Some(expected);

            var result = extractor(option);

            result.Should().Be(expected);
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

            option.GetValueOrThrow(AssertIsNotCalled);
        }

        private static IEnumerable<Func<Optional<TSource>, TResult>> GetMethods<TSource, TResult>()
            where TSource : class, TResult
        {
            yield return option => option.GetValueOrThrow<TResult>();
            yield return option => option.GetValueOrThrow<TResult>(new MyException());
            yield return option => option.GetValueOrThrow<TResult>(() => new MyException());
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return GetMethods<Child, Base>()
                .Select(method => new TestCaseData(method));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Some(Func<Optional<Child>, Base> callGetOrThrow)
        {
            Child expected = new();
            var option = Optional<Child>.Some(expected);

            var value = callGetOrThrow(option);

            value.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_None(Func<Optional<Child>, Base> callGetOrThrow)
        {
            var option = Optional<Child>.None();

            Func<Base> action = () => callGetOrThrow(option);

            action.Should().Throw<Exception>();
        }

        private class MyException : Exception
        {
        }
    }
}
