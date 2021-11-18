using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Value.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(StringFaultResult<string?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(StringFaultResult.Fail<string?>(new("foo")), false),
            CreateReturnBooleanCase(StringFaultResult.Succeed<string?>(null), true),
            CreateReturnBooleanCase(StringFaultResult.Succeed<string?>("bar"), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(StringFaultResult<string?> result)
        {
            return result.TryGetValue(out _);
        }

        private static TestCaseData CreateGetCase(string? expectedValue)
        {
            return new(StringFaultResult.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase("foo"),
        };

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Value(StringFaultResult<string?> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Value_With_If(StringFaultResult<string?> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
