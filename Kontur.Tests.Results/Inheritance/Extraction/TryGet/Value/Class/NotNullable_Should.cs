using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Value.Class
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(StringFaultResult<string> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(StringFaultResult.Fail<string>(new("foo")),  false),
            Create(StringFaultResult.Succeed("bar"), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(StringFaultResult<string> result)
        {
            return result.TryGetValue(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(StringFaultResult.Succeed("foo")) { ExpectedResult = "foo" },
        };

        [TestCaseSource(nameof(GetCases))]

        public string? Extract_Value(StringFaultResult<string> result)
        {
            _ = result.TryGetValue(out var value);

            return value;
        }

        [TestCaseSource(nameof(GetCases))]
        public string Extract_Value_With_If(StringFaultResult<string> result)
        {
            if (result.TryGetValue(out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }
    }
}