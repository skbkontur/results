using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Optional.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Optional<string?> optional, bool success)
        {
            return Common.CreateReturnBooleanCase(optional, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Optional<string?>.None(), false),
            CreateReturnBooleanCase(Optional<string?>.Some(null), true),
            CreateReturnBooleanCase(Optional<string?>.Some("foo"), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Optional<string?> optional)
        {
            return optional.TryGetValue(out _);
        }

        private static TestCaseData CreateGetCase(string? expectedValue)
        {
            return new(Optional<string?>.Some(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase("foo"),
        };

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Value(Optional<string?> optional)
        {
            _ = optional.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public string? Extract_Value_With_If(Optional<string?> optional)
        {
            if (optional.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
