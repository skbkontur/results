using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Value.Class
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
            CreateReturnBooleanCase(Result<string?, string?>.Fail(null), false),
            CreateReturnBooleanCase(Result<string?, string?>.Fail("foo"), false),
            CreateReturnBooleanCase(Result<string?, string?>.Succeed(null), true),
            CreateReturnBooleanCase(Result<string?, string?>.Succeed("bar"), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<string?, string?> result)
        {
            return result.TryGetValue(out _);
        }

        private static TestCaseData CreateGetCase(string? expectedValue)
        {
            return new(Result<string?, string?>.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase("foo"),
        };

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Value(Result<string?, string?> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Value_With_If(Result<string?, string?> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
