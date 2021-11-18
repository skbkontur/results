using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Fault.Class
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
            CreateReturnBooleanCase(Result<string?, string?>.Succeed("foo"), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<string?, string?> result)
        {
            return result.TryGetFault(out _);
        }

        private static TestCaseData CreateGetCase(string? expectedFault)
        {
            return new(Result<string?, string?>.Fail(expectedFault)) { ExpectedResult = expectedFault };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase("foo"),
        };

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Fault(Result<string?, string?> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Fault_With_If(Result<string?, string?> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
