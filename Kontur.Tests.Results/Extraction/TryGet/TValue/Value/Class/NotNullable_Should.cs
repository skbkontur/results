using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Value.Class
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(Result<string, string> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<string, string>.Fail("foo"),  false),
            Create(Result<string, string>.Succeed("bar"), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Result<string, string> result)
        {
            return result.TryGetValue(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Result<string, string>.Succeed("foo")) { ExpectedResult = "foo" },
        };

        [TestCaseSource(nameof(GetCases))]

        public string? Extract_Value(Result<string, string> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public string Extract_Value_With_If(Result<string, string> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}