using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.FaultValue.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Result<string?, string?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Result<string?, string?>.Fail(null), true),
            CreateReturnBooleanCase(Result<string?, string?>.Fail("foo"), true),
            CreateReturnBooleanCase(Result<string?, string?>.Succeed(null), false),
            CreateReturnBooleanCase(Result<string?, string?>.Succeed("bar"), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<string?, string?> result)
        {
            return result.TryGetFault(out _, out _);
        }

        private static TestCaseData CreateFailureCase(string? expectedValue)
        {
            return new(Result<string?, string?>.Fail(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetFailureCases =
        {
            CreateFailureCase(null),
            CreateFailureCase("bar"),
        };

        [TestCaseSource(nameof(GetFailureCases))]
        public string? Extract_Fault(Result<string?, string?> result)
        {
            _ = result.TryGetFault(out var fault, out _);

            return fault;
        }

        [TestCaseSource(nameof(GetFailureCases))]
        public string? Extract_Fault_With_If(Result<string?, string?> result)
        {
            if (result.TryGetFault(out var fault, out _))
            {
                return fault;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateSuccessCase(string? expectedValue)
        {
            return new(Result<string?, string?>.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetSuccessCases =
        {
            CreateSuccessCase(null),
            CreateSuccessCase("foo"),
        };

        [TestCaseSource(nameof(GetSuccessCases))]
        public string? Extract_Value(Result<string?, string?> result)
        {
            _ = result.TryGetFault(out _, out var value);

            return value;
        }

        [TestCaseSource(nameof(GetSuccessCases))]
        public string? Extract_Value_With_If(Result<string?, string?> result)
        {
            if (!result.TryGetFault(out _, out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateAllCase(Result<string?, string?> result, string? expected)
        {
            return new(result) { ExpectedResult = expected };
        }

        private static readonly TestCaseData[] GetAllCases =
        {
            CreateAllCase(Result<string?, string?>.Succeed(null), null),
            CreateAllCase(Result<string?, string?>.Succeed("foo"), "foo"),
            CreateAllCase(Result<string?, string?>.Fail(null), null),
            CreateAllCase(Result<string?, string?>.Fail("bar"), "bar"),
        };

        [TestCaseSource(nameof(GetAllCases))]
        public string? Extract_All_With_If(Result<string?, string?> result)
        {
            return result.TryGetFault(out var fault, out var value)
                ? fault
                : value;
        }
    }
}
