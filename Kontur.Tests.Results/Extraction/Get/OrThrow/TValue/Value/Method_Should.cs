using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Value
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Func<Result<string, int>, int> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(result => result.GetValueOrThrow(new MyException())),
            CreateCase(result => result.GetValueOrThrow(() => new MyException())),
            CreateCase(result => result.GetValueOrThrow(_ => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Failure(Func<Result<string, int>, int> extractor)
        {
            var result = Result<string, int>.Fail("err");

            Func<int> action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Fault_To_Exception_Factory()
        {
            const string expected = "example";
            var result = Result<string, int>.Fail(expected);

            Func<int> action = () => result.GetValueOrThrow(fault => new(fault));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.GetValueOrThrow()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Return_Value_If_Success(Func<Result<string, int>, int> extractor)
        {
            const int expected = 5;
            var source = Result<string, int>.Succeed(expected);

            var result = extractor(source);

            result.Should().Be(expected);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<int, string>,  string> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetValueOrThrow(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetValueOrThrow(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Success(Func<Result<int, string>, string> assertExtracted)
        {
            var result = Result<int, string>.Succeed("result");

            assertExtracted(result);
        }

        private static IEnumerable<Func<Result<string, TSource>, TResult>> GetMethods<TSource, TResult>()
            where TSource : class, TResult
        {
            yield return result => result.GetValueOrThrow<object, TResult>();
            yield return result => result.GetValueOrThrow<object, TResult>(new MyException());
            yield return result => result.GetValueOrThrow<object, TResult>(() => new MyException());
            yield return result => result.GetValueOrThrow<object, TResult>(_ => new MyException());
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return GetMethods<Child, Base>()
                .Select(method => new TestCaseData(method));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Success(Func<Result<string, Child>, Base> callGetOrThrow)
        {
            Child expected = new();
            var result = Result<string, Child>.Succeed(expected);

            var value = callGetOrThrow(result);

            value.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Failure(Func<Result<string, Child>, Base> callGetOrThrow)
        {
            var result = Result<string, Child>.Fail("unused");

            Func<Base> action = () => callGetOrThrow(result);

            action.Should().Throw<Exception>();
        }

        private class MyException : Exception
        {
        }
    }
}
