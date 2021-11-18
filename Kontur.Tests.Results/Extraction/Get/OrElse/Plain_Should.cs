using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrElse
{
    [TestFixture]
    internal class Plain_Should
    {
        private static int AssertIsNotCalled()
        {
            Assert.Fail("Backup fault factory should not be called on Success");
            throw new UnreachableException();
        }

        [Test]
        public void Do_Not_Call_Delegate_If_Fault()
        {
            var result = Result<int>.Fail(0);

            result.GetFaultOrElse(AssertIsNotCalled);
        }

        private static IEnumerable<Func<Result<TSource>, TResult>> GetMethods<TSource, TResult>(TResult defaultFault)
            where TSource : class, TResult
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetStringCases()
        {
            const string defaultFault = "default on success";
            const string someFault = "bar";

            (Result<string> Source, string Result)[] testCases =
            {
                (Result<string>.Succeed(), defaultFault),
                (Result<string>.Fail(someFault), someFault),
            };

            return
                from testCase in testCases
                from method in GetMethods<string, string>(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetStringCases))]
        public string Return_Result(Result<string> result, Func<Result<string>, string> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            Base defaultFault = new();

            return
                from testCase in UpcastPlainExamples.Get(value => value, defaultFault)
                from method in GetMethods<Child, Base>(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public Base Upcast(Result<Child> result, Func<Result<Child>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<Func<Result<Base>, Base>> GetUpcastDefaultFaultMethods(Child defaultFault)
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetUpcastDefaultFaultCases()
        {
            Child defaultFault = new();
            Base someFault = new();

            (Result<Base> Source, Base Result)[] cases =
            {
                (Result<Base>.Fail(someFault), someFault),
                (Result<Base>.Succeed(), defaultFault),
            };

            return
                from testCase in cases
                from method in GetUpcastDefaultFaultMethods(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastDefaultFaultCases))]
        public Base Upcast_Default_Fault(Result<Base> result, Func<Result<Base>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }
    }
}
