using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Value.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(StringFaultResult<int> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(StringFaultResult.Fail<int>(new("err")), false),
            Create(StringFaultResult.Succeed(42), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(StringFaultResult<int> result)
        {
            return result.TryGetValue(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(StringFaultResult.Succeed(10)) { ExpectedResult = 10 },
        };

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Value(StringFaultResult<int> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Value_With_If(StringFaultResult<int> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
