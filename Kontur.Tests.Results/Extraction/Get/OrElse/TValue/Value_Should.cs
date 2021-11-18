using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrElse.TValue
{
    [TestFixture]
    internal class Value_Should
    {
        private static int AssertIsNotCalled()
        {
            Assert.Fail("Backup value factory should not be called on Success");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<Guid, int>, int> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetValueOrElse(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetValueOrElse(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_Not_Call_Delegate_If_Success(Func<Result<Guid, int>, int> assertExtracted)
        {
            var result = Result<Guid, int>.Succeed(0);

            assertExtracted(result);
        }

        [Test]
        public void Pass_Fault_To_Factory()
        {
            var result = Result<int, string>.Fail(23);

            var value = result.GetValueOrElse(fault => fault.ToString(CultureInfo.InvariantCulture));

            value.Should().Be("23");
        }

        private static IEnumerable<Func<Result<string, TSource>, TResult>> GetMethods<TSource, TResult>(TResult defaultValue)
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

            (Result<string, string> Source, string Result)[] testCases =
            {
                (Result<string, string>.Fail("unused"), defaultValue),
                (Result<string, string>.Succeed(someValue), someValue),
            };

            return
                from testCase in testCases
                from method in GetMethods<string, string>(defaultValue)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetStringCases))]
        public string Return_Result(Result<string, string> result, Func<Result<string, string>, string> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            Base defaultValue = new();

            return
                from testCase in UpcastTValueExamples.GetValues(_ => defaultValue, value => value)
                from method in GetMethods<Child, Base>(defaultValue)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public Base Upcast(Result<string, Child> result, Func<Result<string, Child>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<Func<Result<string, Base>, Base>> GetUpcastDefaultValueMethods(Child defaultValue)
        {
            yield return result => result.GetValueOrElse(defaultValue);
            yield return result => result.GetValueOrElse(() => defaultValue);
            yield return result => result.GetValueOrElse(_ => defaultValue);
        }

        private static IEnumerable<TestCaseData> GetUpcastDefaultValueCases()
        {
            Child defaultValue = new();
            Base someValue = new();

            (Result<string, Base> Source, Base Result)[] cases =
            {
                (Result<string, Base>.Succeed(someValue), someValue),
                (Result<string, Base>.Fail("unused"), defaultValue),
            };

            return
                from testCase in cases
                from method in GetUpcastDefaultValueMethods(defaultValue)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastDefaultValueCases))]
        public Base Upcast_Default_Value(Result<string, Base> result, Func<Result<string, Base>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }
    }
}
