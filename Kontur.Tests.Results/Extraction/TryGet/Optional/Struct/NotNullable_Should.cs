using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Optional.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(Optional<int> optional, bool success)
        {
            return Common.CreateReturnBooleanCase(optional, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Optional<int>.None(), false),
            Create(Optional<int>.Some(1), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Optional<int> optional)
        {
            return optional.TryGetValue(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Optional<int>.Some(10)) { ExpectedResult = 10 },
        };

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Value(Optional<int> optional)
        {
            _ = optional.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Value_With_If(Optional<int> optional)
        {
            if (optional.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}
