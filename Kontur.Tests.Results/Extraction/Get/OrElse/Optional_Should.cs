using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrElse
{
    [TestFixture]
    internal class Optional_Should
    {
        private static int AssertIsNotCalled()
        {
            Assert.Fail("Backup value factory should not be called on Some");
            throw new UnreachableException();
        }

        [Test]
        public void Do_Not_Call_Delegate_If_Some()
        {
            var option = Optional<int>.Some(0);

            option.GetValueOrElse(AssertIsNotCalled);
        }

        private static IEnumerable<Func<Optional<TSource>, TResult>> GetMethods<TSource, TResult>(TResult defaultValue)
            where TSource : class, TResult
        {
            yield return option => option.GetValueOrElse(defaultValue);
            yield return option => option.GetValueOrElse(() => defaultValue);
        }

        private static IEnumerable<TestCaseData> GetStringCases()
        {
            const string defaultValue = "default on none";
            const string someValue = "bar";

            var testCases = new (Optional<string> Option, string Result)[]
            {
                (Optional<string>.None(), defaultValue),
                (Optional<string>.Some(someValue), someValue),
            };

            return
                from testCase in testCases
                from method in GetMethods<string, string>(defaultValue)
                select new TestCaseData(testCase.Option, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetStringCases))]
        public string Return_Result(Optional<string> optional, Func<Optional<string>, string> callGetOrElse)
        {
            return callGetOrElse(optional);
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            Base defaultValue = new();

            return
                from testCase in UpcastOptionalExamples.Get(defaultValue, value => value)
                from method in GetMethods<Child, Base>(defaultValue)
                select new TestCaseData(testCase.Optional, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public Base Upcast(Optional<Child> optional, Func<Optional<Child>, Base> callGetOrElse)
        {
            return callGetOrElse(optional);
        }

        private static IEnumerable<Func<Optional<Base>, Base>> GetUpcastDefaultValueMethods(Child defaultValue)
        {
            yield return option => option.GetValueOrElse(defaultValue);
            yield return option => option.GetValueOrElse(() => defaultValue);
        }

        private static IEnumerable<TestCaseData> GetUpcastDefaultValueCases()
        {
            Child defaultValue = new();
            Base someValue = new();

            (Optional<Base> Option, Base Result)[] cases =
            {
                (Optional<Base>.Some(someValue), someValue),
                (Optional<Base>.None(), defaultValue),
            };

            return
                from testCase in cases
                from method in GetUpcastDefaultValueMethods(defaultValue)
                select new TestCaseData(testCase.Option, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastDefaultValueCases))]
        public Base Upcast_Default_Value(Optional<Base> optional, Func<Optional<Base>, Base> callGetOrElse)
        {
            return callGetOrElse(optional);
        }
    }
}
