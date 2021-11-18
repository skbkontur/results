using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Fault.Class
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(StringFaultResult<string?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(StringFaultResult.Fail<string?>(new("foo")), true),
            CreateReturnBooleanCase(StringFaultResult.Succeed<string?>(null), false),
            CreateReturnBooleanCase(StringFaultResult.Succeed<string?>("foo"), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(StringFaultResult<string?> result)
        {
            return result.TryGetFault(out _);
        }

        private static TestCaseData CreateGetCase(StringFault expectedFault)
        {
            return new(StringFaultResult.Fail<string?>(expectedFault)) { ExpectedResult = expectedFault };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(new("foo")),
        };

        [TestCaseSource(nameof(GetCases))]
        public StringFault? Extract_Fault(StringFaultResult<string?> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault Extract_Fault_With_If(StringFaultResult<string?> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
