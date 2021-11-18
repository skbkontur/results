using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Conversion.Combinations.Select
{
    [TestFixture]
    internal class Value_Should
    {
        private static TestCaseData Create(StringFaultResult<int> source, Result<StringFault, string> result)
        {
            return new(source) { ExpectedResult = result };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            StringFault faultExample = new("foo");
            yield return Create(StringFaultResult.Fail<int>(faultExample), Result<StringFault, string>.Fail(faultExample));
            yield return Create(StringFaultResult.Succeed(1), Result<StringFault, string>.Succeed("1"));
        }

        [TestCaseSource(nameof(GetCases))]
        public Result<StringFault, string> Process_Value(StringFaultResult<int> result)
        {
            return result.Select(i => i.ToString(CultureInfo.InvariantCulture));
        }

        [TestCaseSource(nameof(GetCases))]
        public Result<StringFault, string> Process_Result(StringFaultResult<int> result)
        {
            return result.Select(i => Result<StringFault, string>.Succeed(i.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void Convert_Success_To_Failure()
        {
            var source = StringFaultResult.Succeed(Guid.NewGuid());

            StringFault fault = new("bar");
            var result = source.Select(_ => Result<StringFault, int>.Fail(fault));

            result.Should().BeEquivalentTo(Result<StringFault, int>.Fail(fault));
        }
    }
}
