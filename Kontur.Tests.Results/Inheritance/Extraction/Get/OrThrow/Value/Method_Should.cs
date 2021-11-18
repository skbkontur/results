using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrThrow.Value
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Func<StringFaultResult<int>, int> extractor)
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
        public void Throw_MyException_If_Failure(Func<StringFaultResult<int>, int> extractor)
        {
            var result = StringFaultResult.Fail<int>(new("err"));

            Func<int> action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Fault_To_Exception_Factory()
        {
            const string expected = "example";
            var result = StringFaultResult.Fail<int>(new(expected));

            Func<int> action = () => result.GetValueOrThrow(fault => new(fault.ToString()));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.GetValueOrThrow()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Return_Value_If_Success(Func<StringFaultResult<int>, int> extractor)
        {
            const int expected = 5;
            var source = StringFaultResult.Succeed(expected);

            var result = extractor(source);

            result.Should().Be(expected);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<StringFaultResult<int>,  int> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetValueOrThrow(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetValueOrThrow(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Success(Func<StringFaultResult<int>, int> assertExtracted)
        {
            var result = StringFaultResult.Succeed(50);

            assertExtracted(result);
        }

        private static IEnumerable<Func<StringFaultResult<TSource>, TResult>> GetMethods<TSource, TResult>()
            where TSource : class, TResult
        {
            yield return result => result.GetValueOrThrow<StringFault, TResult>();
            yield return result => result.GetValueOrThrow<StringFault, TResult>(new MyException());
            yield return result => result.GetValueOrThrow<StringFault, TResult>(() => new MyException());
            yield return result => result.GetValueOrThrow<StringFault, TResult>(_ => new MyException());
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return GetMethods<Child, Base>()
                .Select(method => new TestCaseData(method));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Success(Func<StringFaultResult<Child>, Base> callGetOrThrow)
        {
            Child expected = new();
            var result = StringFaultResult.Succeed(expected);

            var value = callGetOrThrow(result);

            value.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Failure(Func<StringFaultResult<Child>, Base> callGetOrThrow)
        {
            var result = StringFaultResult.Fail<Child>(new("unused"));

            Func<Base> action = () => callGetOrThrow(result);

            action.Should().Throw<Exception>();
        }

        private class MyException : Exception
        {
        }
    }
}
