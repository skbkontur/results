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
    internal class Fault_Should
    {
        private static int AssertIsNotCalled()
        {
            Assert.Fail("Backup fault factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<Result<int, Guid>, int> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetFaultOrElse(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetFaultOrElse(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_Not_Call_Delegate_If_Failure(Func<Result<int, Guid>, int> assertExtracted)
        {
            var result = Result<int, Guid>.Fail(0);

            assertExtracted(result);
        }

        [Test]
        public void Pass_Value_To_Factory()
        {
            var result = Result<string, int>.Succeed(23);

            var fault = result.GetFaultOrElse(value => value.ToString(CultureInfo.InvariantCulture));

            fault.Should().Be("23");
        }

        private static IEnumerable<Func<Result<TSource, string>, TResult>> GetMethods<TSource, TResult>(TResult defaultFault)
            where TSource : class, TResult
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
            yield return result => result.GetFaultOrElse(_ => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetStringCases()
        {
            const string defaultFault = "default on success";
            const string someFault = "bar";

            (Result<string, string> Source, string Result)[] testCases =
            {
                (Result<string, string>.Succeed("unused"), defaultFault),
                (Result<string, string>.Fail(someFault), someFault),
            };

            return
                from testCase in testCases
                from method in GetMethods<string, string>(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetStringCases))]
        public string Return_Result(Result<string, string> result, Func<Result<string, string>, string> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            Base defaultFault = new();

            return
                from testCase in UpcastTValueExamples.GetFaults(value => value, _ => defaultFault)
                from method in GetMethods<Child, Base>(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public Base Upcast(Result<Child, string> result, Func<Result<Child, string>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<Func<Result<Base, string>, Base>> GetUpcastDefaultFaultMethods(Child defaultFault)
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
            yield return result => result.GetFaultOrElse(_ => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetUpcastDefaultFaultCases()
        {
            Child defaultFault = new();
            Base someFault = new();

            (Result<Base, string> Source, Base Result)[] cases =
            {
                (Result<Base, string>.Fail(someFault), someFault),
                (Result<Base, string>.Succeed("unused"), defaultFault),
            };

            return
                from testCase in cases
                from method in GetUpcastDefaultFaultMethods(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastDefaultFaultCases))]
        public Base Upcast_Default_Fault(Result<Base, string> result, Func<Result<Base, string>, Base> callGetOrElse)
        {
            return callGetOrElse(result);
        }
    }
}
