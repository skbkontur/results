using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Optional.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Optional<int?> optional, bool success)
        {
            return Common.CreateReturnBooleanCase(optional, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Optional<int?>.None(), false),
            CreateReturnBooleanCase(Optional<int?>.Some(null), true),
            CreateReturnBooleanCase(Optional<int?>.Some(1), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Optional<int?> optional)
        {
            return optional.TryGetValue(out _);
        }

        private static TestCaseData CreateGetCase(int? expectedValue)
        {
            return new(Optional<int?>.Some(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase(10),
        };

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Value(Optional<int?> optional)
        {
            _ = optional.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Value_With_If(Optional<int?> optional)
        {
            if (optional.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
