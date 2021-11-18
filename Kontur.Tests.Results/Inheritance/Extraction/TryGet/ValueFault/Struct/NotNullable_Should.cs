using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.ValueFault.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(StringFaultResult<int> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(StringFaultResult.Fail<int>(new("foo")), false),
            CreateReturnBooleanCase(StringFaultResult.Succeed(1), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(StringFaultResult<int> result)
        {
            return result.TryGetValue(out _, out _);
        }

        private static readonly TestCaseData[] GetSuccessCases =
        {
            new(StringFaultResult.Succeed(10)) { ExpectedResult = 10 },
        };

        [TestCaseSource(nameof(GetSuccessCases))]
        public int Extract_Value(StringFaultResult<int> result)
        {
            _ = result.TryGetValue(out var value, out _);

            return value;
        }

        [TestCaseSource(nameof(GetSuccessCases))]
        public int Extract_Value_With_If(StringFaultResult<int> result)
        {
            if (result.TryGetValue(out var value, out _))
            {
                return value;
            }

            throw new UnreachableException();
        }

        private static IEnumerable<TestCaseData> GetFailureCases()
        {
            StringFault fault = new("bar");
            yield return new(StringFaultResult.Fail<int>(fault)) { ExpectedResult = fault };
        }

        [TestCaseSource(nameof(GetFailureCases))]
        public StringFault? Extract_Fault(StringFaultResult<int> result)
        {
            _ = result.TryGetValue(out _, out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetFailureCases))]
        public StringFault Extract_Fault_With_If(StringFaultResult<int> result)
        {
            if (!result.TryGetValue(out _, out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateAllCase(StringFaultResult<int> result, string expected)
        {
            return new(result) { ExpectedResult = expected };
        }

        private static readonly TestCaseData[] GetAllCases =
        {
            CreateAllCase(StringFaultResult.Fail<int>(new("bar")), "bar"),
            CreateAllCase(StringFaultResult.Succeed(33), "33"),
        };

        [TestCaseSource(nameof(GetAllCases))]
        public string Extract_All_With_If(StringFaultResult<int> result)
        {
            return result.TryGetValue(out var value, out var fault)
                ? value.ToString(CultureInfo.InvariantCulture)
                : fault.ToString();
        }
    }
}
