using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Value.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(Result<int, int> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<int, int>.Fail(3), false),
            Create(Result<int, int>.Succeed(42), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Result<int, int> result)
        {
            return result.TryGetValue(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Result<int, int>.Succeed(10)) { ExpectedResult = 10 },
        };

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Value(Result<int, int> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Value_With_If(Result<int, int> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
