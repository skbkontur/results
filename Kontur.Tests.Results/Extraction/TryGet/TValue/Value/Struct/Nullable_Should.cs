using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Value.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Result<int?, int?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Result<int?, int?>.Fail(null), false),
            CreateReturnBooleanCase(Result<int?, int?>.Fail(33), false),
            CreateReturnBooleanCase(Result<int?, int?>.Succeed(null), true),
            CreateReturnBooleanCase(Result<int?, int?>.Succeed(1), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<int?, int?> result)
        {
            return result.TryGetValue(out _);
        }

        private static TestCaseData CreateGetCase(int? expectedValue)
        {
            return new(Result<int?, int?>.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase(10),
        };

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Value(Result<int?, int?> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Value_With_If(Result<int?, int?> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
