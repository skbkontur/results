using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Plain
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Func<Result<int>, int> extractor)
        {
            return new(extractor);
        }

        private static readonly TestCaseData[] MyExceptionCases =
        {
            CreateCase(result => result.GetFaultOrThrow(new MyException())),
            CreateCase(result => result.GetFaultOrThrow(() => new MyException())),
        };

        [TestCaseSource(nameof(MyExceptionCases))]
        public void Throw_MyException_If_Success(Func<Result<int>, int> extractor)
        {
            var result = Result<int>.Succeed();

            Func<int> action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        private static readonly IEnumerable<TestCaseData> FailureCases = MyExceptionCases
            .Append(CreateCase(result => result.GetFaultOrThrow()));

        [TestCaseSource(nameof(FailureCases))]
        public void Return_Fault_If_Failure(Func<Result<int>, int> extractor)
        {
            const int expected = 5;
            var result = Result<int>.Fail(expected);

            var fault = extractor(result);

            fault.Should().Be(expected);
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

            result.GetFaultOrThrow(AssertIsNotCalled);
        }

        private static IEnumerable<Func<Result<TSource>, TResult>> GetMethods<TSource, TResult>()
            where TSource : class, TResult
        {
            yield return result => result.GetFaultOrThrow<TResult>();
            yield return result => result.GetFaultOrThrow<TResult>(new MyException());
            yield return result => result.GetFaultOrThrow<TResult>(() => new MyException());
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return GetMethods<Child, Base>()
                .Select(method => new TestCaseData(method));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Failure(Func<Result<Child>, Base> callGetOrThrow)
        {
            Child expected = new();
            var result = Result<Child>.Fail(expected);

            var value = callGetOrThrow(result);

            value.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Success(Func<Result<Child>, Base> callGetOrThrow)
        {
            var result = Result<Child>.Succeed();

            Func<Base> action = () => callGetOrThrow(result);

            action.Should().Throw<Exception>();
        }

        private class MyException : Exception
        {
        }
    }
}
