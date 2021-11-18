using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Value.Struct
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
            CreateReturnBooleanCase(StringFaultResult.Fail<int?>(new("error")), false),
            CreateReturnBooleanCase(StringFaultResult.Succeed<int?>(null), true),
            CreateReturnBooleanCase(StringFaultResult.Succeed<int?>(1), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(StringFaultResult<int?> result)
        {
            return result.TryGetValue(out _);
        }

        private static TestCaseData CreateGetCase(int? expectedValue)
        {
            return new(StringFaultResult.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase(10),
        };

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Value(StringFaultResult<int?> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Value_With_If(StringFaultResult<int?> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
