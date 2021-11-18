using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Optional.Class
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(Optional<string> optional, bool success)
        {
            return Common.CreateReturnBooleanCase(optional, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Optional<string>.None(), false),
            Create(Optional<string>.Some("foo"), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Optional<string> optional)
        {
            return optional.TryGetValue(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Optional<string>.Some("foo")) { ExpectedResult = "foo" },
        };

        [TestCaseSource(nameof(GetCases))]

        public string? Extract_Value(Optional<string> optional)
        {
            _ = optional.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public string Extract_Value_With_If(Optional<string> optional)
        {
            if (optional.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}