using System.Globalization;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.FaultValue.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(StringFaultResult<int?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(StringFaultResult.Fail<int?>(new("error")), true),
            CreateReturnBooleanCase(StringFaultResult.Succeed<int?>(null), false),
            CreateReturnBooleanCase(StringFaultResult.Succeed<int?>(1), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(StringFaultResult<int?> result)
        {
            return result.TryGetFault(out _, out _);
        }

        private static TestCaseData CreateSuccessCase(int? expectedValue)
        {
            return new(StringFaultResult.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetSuccessCases =
        {
            CreateSuccessCase(null),
            CreateSuccessCase(10),
        };

        [TestCaseSource(nameof(GetSuccessCases))]
        public int? Extract_Value(StringFaultResult<int?> result)
        {
            _ = result.TryGetFault(out _, out var value);

            return value;
        }

        [TestCaseSource(nameof(GetSuccessCases))]
        public int? Extract_Value_With_If(StringFaultResult<int?> result)
        {
            if (!result.TryGetFault(out _, out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateFailureCase(StringFault expectedValue)
        {
            return new(StringFaultResult.Fail<int?>(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetFailureCases =
        {
            CreateFailureCase(new("bar")),
        };

        [TestCaseSource(nameof(GetFailureCases))]
        public StringFault? Extract_Fault(StringFaultResult<int?> result)
        {
            _ = result.TryGetFault(out var fault, out _);

            return fault;
        }

        [TestCaseSource(nameof(GetFailureCases))]
        public StringFault Extract_Fault_With_If(StringFaultResult<int?> result)
        {
            if (result.TryGetFault(out var fault, out _))
            {
                return fault;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateAllCase(StringFaultResult<int?> result, string expected)
        {
            return new(result) { ExpectedResult = expected };
        }

        private static readonly TestCaseData[] GetAllCases =
        {
            CreateAllCase(StringFaultResult.Fail<int?>(new("foo")), "foo"),
            CreateAllCase(StringFaultResult.Succeed<int?>(null), "null"),
            CreateAllCase(StringFaultResult.Succeed<int?>(33), "33"),
        };

        [TestCaseSource(nameof(GetAllCases))]
        public string Extract_All_With_If(StringFaultResult<int?> result)
        {
            return result.TryGetFault(out var fault, out var value)
                ? fault.ToString()
                : value?.ToString(CultureInfo.InvariantCulture) ?? "null";
        }
    }
}
