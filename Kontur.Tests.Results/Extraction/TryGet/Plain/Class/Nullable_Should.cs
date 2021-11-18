using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Plain.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Result<string?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Result<string?>.Succeed(), false),
            CreateReturnBooleanCase(Result<string?>.Fail(null), true),
            CreateReturnBooleanCase(Result<string?>.Fail("foo"), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<string?> result)
        {
            return result.TryGetFault(out _);
        }

        private static TestCaseData CreateGetCase(string? expectedValue)
        {
            return new(Result<string?>.Fail(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase("foo"),
        };

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Fault(Result<string?> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Fault_With_If(Result<string?> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
