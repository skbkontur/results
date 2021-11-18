using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Fault.Class
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
            Create(Result<string, string>.Fail("foo"), true),
            Create(Result<string, string>.Succeed("bar"), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Result<string, string> result)
        {
            return result.TryGetFault(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Result<string, string>.Fail("foo")) { ExpectedResult = "foo" },
        };

        [TestCaseSource(nameof(GetCases))]

        public string? Extract_Fault(Result<string, string> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public string Extract_Fault_With_If(Result<string, string> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}