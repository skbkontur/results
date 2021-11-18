using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Fault.Struct
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
            CreateReturnBooleanCase(StringFaultResult.Fail<int?>(new("error")), true),
            CreateReturnBooleanCase(StringFaultResult.Succeed<int?>(null), false),
            CreateReturnBooleanCase(StringFaultResult.Succeed<int?>(33), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(StringFaultResult<int?> result)
        {
            return result.TryGetFault(out _);
        }

        private static TestCaseData CreateGetCase(StringFault expectedFault)
        {
            return new(StringFaultResult.Fail<int?>(expectedFault)) { ExpectedResult = expectedFault };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(new("error")),
        };

        [TestCaseSource(nameof(GetCases))]
        public StringFault? Extract_Fault(StringFaultResult<int?> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault Extract_Fault_With_If(StringFaultResult<int?> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
