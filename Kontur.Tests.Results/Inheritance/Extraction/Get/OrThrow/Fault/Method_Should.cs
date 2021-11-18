using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrThrow.Fault
{
    [TestFixture]
    internal class Method_Should
    {
        private static TestCaseData CreateCase(Func<StringFaultResult<int>, StringFault> extractor)
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
        public void Throw_MyException_If_Success(Func<StringFaultResult<int>, StringFault> extractor)
        {
            var result = StringFaultResult.Succeed(50);

            Func<StringFault> action = () => extractor(result);

            action.Should().Throw<MyException>();
        }

        [Test]
        public void Pass_Value_To_Exception_Factory()
        {
            const string expected = "passed";
            var result = StringFaultResult.Succeed(expected);

            Func<StringFault> action = () => result.GetFaultOrThrow(value => new(value));

            action.Should()
                .Throw<Exception>()
                .WithMessage(expected);
        }

        private static readonly IEnumerable<TestCaseData> SuccessCases = MyExceptionCases
            .Append(CreateCase(result => result.GetFaultOrThrow()));

        [TestCaseSource(nameof(SuccessCases))]
        public void Return_Fault_If_Failure(Func<StringFaultResult<int>, StringFault> extractor)
        {
            StringFault expected = new("bar");
            var source = StringFaultResult.Fail<int>(expected);

            var result = extractor(source);

            result.Should().Be(expected);
        }

        private static Exception AssertIsNotCalled()
        {
            Assert.Fail("Exception should not be created on failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<StringFaultResult<int>, StringFault> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetFaultOrThrow(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetFaultOrThrow(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_No_Create_Exception_If_Failure(Func<StringFaultResult<int>, StringFault> assertExtracted)
        {
            var result = StringFaultResult.Fail<int>(new("fault"));

            assertExtracted(result);
        }

        private static IEnumerable<Func<StringFaultResult<string>, StringFaultBase>> GetMethods()
        {
            yield return result => result.GetFaultOrThrow<StringFaultBase, string>();
            yield return result => result.GetFaultOrThrow<StringFaultBase, string>(new MyException());
            yield return result => result.GetFaultOrThrow<StringFaultBase, string>(() => new MyException());
            yield return result => result.GetFaultOrThrow<StringFaultBase, string>(_ => new MyException());
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            return GetMethods().Select(method => new TestCaseData(method));
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Failure(Func<StringFaultResult<string>, StringFaultBase> callGetOrThrow)
        {
            StringFault expected = new("bar");
            var result = StringFaultResult.Fail<string>(expected);

            var value = callGetOrThrow(result);

            value.Should().Be(expected);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public void Upcast_On_Success(Func<StringFaultResult<string>, StringFaultBase> callGetOrThrow)
        {
            var result = StringFaultResult.Succeed("unused");

            Func<StringFaultBase> action = () => callGetOrThrow(result);

            action.Should().Throw<Exception>();
        }

        private class MyException : Exception
        {
        }
    }
}
