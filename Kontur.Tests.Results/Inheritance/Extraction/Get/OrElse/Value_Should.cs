using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrElse
{
    [TestFixture]
    internal class Value_Should
    {
        private static int AssertIsNotCalled()
        {
            Assert.Fail("Backup value factory should not be called on Success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<StringFaultResult<int>, int> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetValueOrElse(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetValueOrElse(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_Not_Call_Delegate_If_Success(Func<StringFaultResult<int>, int> assertExtracted)
        {
            var result = StringFaultResult.Succeed(0);

            assertExtracted(result);
        }

        [Test]
        public void Pass_Fault_To_Factory()
        {
            const string expected = "23";
            var result = StringFaultResult.Fail<string>(new(expected));

            var value = result.GetValueOrElse(fault => fault.ToString());

            value.Should().Be(expected);
        }

        private static IEnumerable<Func<StringFaultResult<TSource>, TResult>> GetMethods<TSource, TResult>(TResult defaultValue)
            where TSource : class, TResult
        {
            yield return result => result.GetValueOrElse(defaultValue);
            yield return result => result.GetValueOrElse(() => defaultValue);
            yield return result => result.GetValueOrElse(_ => defaultValue);
        }

        private static IEnumerable<TestCaseData> GetStringCases()
        {
            const string defaultValue = "default on failure";
            const string someValue = "bar";

            (StringFaultResult<string> Source, string Result)[] testCases =
            {
                (StringFaultResult.Fail<string>(new("unused")), defaultValue),
                (StringFaultResult.Succeed(someValue), someValue),
            };

            return
                from testCase in testCases
                from method in GetMethods<string, string>(defaultValue)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetStringCases))]
        public string Return_Result(StringFaultResult<string> result, Func<StringFaultResult<string>, string> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            Base defaultValue = new();

            return
                from testCase in UpcastExamples.GetValues(_ => defaultValue, value => value)
                from method in GetMethods<Child, Base>(defaultValue)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public Base Upcast(StringFaultResult<Child> result, Func<StringFaultResult<Child>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<Func<StringFaultResult<Base>, Base>> GetUpcastDefaultValueMethods(Child defaultValue)
        {
            yield return result => result.GetValueOrElse(defaultValue);
            yield return result => result.GetValueOrElse(() => defaultValue);
            yield return result => result.GetValueOrElse(_ => defaultValue);
        }

        private static IEnumerable<TestCaseData> GetUpcastDefaultValueCases()
        {
            Child defaultValue = new();
            Base someValue = new();

            (StringFaultResult<Base> Source, Base Result)[] cases =
            {
                (StringFaultResult.Succeed(someValue), someValue),
                (StringFaultResult.Fail<Base>(new("unused")), defaultValue),
            };

            return
                from testCase in cases
                from method in GetUpcastDefaultValueMethods(defaultValue)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastDefaultValueCases))]
        public Base Upcast_Default_Value(StringFaultResult<Base> result, Func<StringFaultResult<Base>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }
    }
}
