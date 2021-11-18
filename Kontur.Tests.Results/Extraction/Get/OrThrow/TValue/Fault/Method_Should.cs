using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Fault
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Func<Result<int, string>, int> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(result => result.GetFaultOrThrow(new MyException())),
            CreateCase(result => result.GetFaultOrThrow(() => new MyException())),
            CreateCase(result => result.GetFaultOrThrow(_ => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Success(Func<Result<int, string>, int> extractor)
        {
            var result = Result<int, string>.Succeed("val");

            Func<int> action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Value_To_Exception_Factory()
        {
            const string expected = "example";
            var result = Result<int, string>.Succeed(expected);

            Func<int> action = () => result.GetFaultOrThrow(value => new(value));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.GetFaultOrThrow()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Return_Fault_If_Failure(Func<Result<int, string>, int> extractor)
        {
            const int expected = 5;
            var source = Result<int, string>.Fail(expected);

            var result = extractor(source);

            result.Should().Be(expected);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<int, string>, int> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetFaultOrThrow(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetFaultOrThrow(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Failure(Func<Result<int, string>, int> assertExtracted)
        {
            var result = Result<int, string>.Fail(5);

            assertExtracted(result);
        }

        private static IEnumerable<Func<Result<TSource, string>, TResult>> GetMethods<TSource, TResult>()
            where TSource : class, TResult
        {
            yield return result => result.GetFaultOrThrow<TResult, object>();
            yield return result => result.GetFaultOrThrow<TResult, object>(new MyException());
            yield return result => result.GetFaultOrThrow<TResult, object>(() => new MyException());
            yield return result => result.GetFaultOrThrow<TResult, object>(_ => new MyException());
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return GetMethods<Child, Base>()
                .Select(method => new TestCaseData(method));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Failure(Func<Result<Child, string>, Base> callGetOrThrow)
        {
            Child expected = new();
            var result = Result<Child, string>.Fail(expected);

            var value = callGetOrThrow(result);

            value.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Success(Func<Result<Child, string>, Base> callGetOrThrow)
        {
            var result = Result<Child, string>.Succeed("unused");

            Func<Base> action = () => callGetOrThrow(result);

            action.Should().Throw<Exception>();
        }

        private class MyException : Exception
        {
        }
    }
}
