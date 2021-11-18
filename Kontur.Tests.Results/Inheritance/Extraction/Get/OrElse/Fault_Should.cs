using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrElse
{
    [TestFixture]
    internal class Fault_Should
    {
        private static StringFault AssertIsNotCalled()
        {
            Assert.Fail("Backup fault factory should not be called on Failure");
            throw new UnreachableException();
        }

        private static TestCaseData CreateDoNoCallFactoryCase(Func<StringFaultResult<Guid>, StringFault> assertExtracted)
        {
            return new(assertExtracted);
        }

        private static readonly TestCaseData[] CreateDoNoCallFactoryOnCases =
        {
            CreateDoNoCallFactoryCase(result => result.GetFaultOrElse(_ => AssertIsNotCalled())),
            CreateDoNoCallFactoryCase(result => result.GetFaultOrElse(AssertIsNotCalled)),
        };

        [TestCaseSource(nameof(CreateDoNoCallFactoryOnCases))]
        public void Do_Not_Call_Delegate_If_Failure(Func<StringFaultResult<Guid>, StringFault> assertExtracted)
        {
            var result = StringFaultResult.Fail<Guid>(new("fail"));

            assertExtracted(result);
        }

        [Test]
        public void Pass_Value_To_Factory()
        {
            var result = StringFaultResult.Succeed(23);

            var fault = result.GetFaultOrElse(value => new(value.ToString(CultureInfo.InvariantCulture)));

            fault.Should().Be(new StringFault("23"));
        }

        private static IEnumerable<Func<StringFaultResult<string>, StringFault>> GetMethods(StringFault defaultFault)
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
            yield return result => result.GetFaultOrElse(_ => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetStringCases()
        {
            StringFault defaultFault = new("default on success");
            StringFault someFault = new("bar");

            (StringFaultResult<string> Source, StringFault Result)[] testCases =
            {
                (StringFaultResult.Succeed("unused"), defaultFault),
                (StringFaultResult.Fail<string>(someFault), someFault),
            };

            return
                from testCase in testCases
                from method in GetMethods(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetStringCases))]
        public StringFault Return_Result(StringFaultResult<string> result, Func<StringFaultResult<string>, StringFault> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<Func<StringFaultResult<string>, StringFaultBase>> GetUpcastMethods(StringFaultBase defaultFault)
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
            yield return result => result.GetFaultOrElse(_ => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetUpcastCases()
        {
            StringFaultBase defaultFault = new("bar");

            return
                from testCase in UpcastExamples.GetFaults(value => value, _ => defaultFault)
                from method in GetUpcastMethods(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastCases))]
        public StringFaultBase Upcast(StringFaultResult<string> result, Func<StringFaultResult<string>, StringFaultBase> callGetOrElse)
        {
            return callGetOrElse(result);
        }

        private static IEnumerable<Func<StringFaultResult<string>, StringFault>> GetUpcastDefaultFaultMethods(StringFaultChild defaultFault)
        {
            yield return result => result.GetFaultOrElse(defaultFault);
            yield return result => result.GetFaultOrElse(() => defaultFault);
            yield return result => result.GetFaultOrElse(_ => defaultFault);
        }

        private static IEnumerable<TestCaseData> GetUpcastDefaultFaultCases()
        {
            StringFaultChild defaultFault = new("foo");
            StringFault someFault = new("bar");

            (StringFaultResult<string> Source, StringFault Result)[] methods =
            {
                (StringFaultResult.Fail<string>(someFault), someFault),
                (StringFaultResult.Succeed("unused"), defaultFault),
            };

            return
                from testCase in methods
                from method in GetUpcastDefaultFaultMethods(defaultFault)
                select new TestCaseData(testCase.Source, method).Returns(testCase.Result);
        }

        [TestCaseSource(nameof(GetUpcastDefaultFaultCases))]
        public StringFault Upcast_Default_Fault(StringFaultResult<string> result, Func<StringFaultResult<string>, StringFault> callGetOrElse)
        {
            return callGetOrElse(result);
        }
    }
}
