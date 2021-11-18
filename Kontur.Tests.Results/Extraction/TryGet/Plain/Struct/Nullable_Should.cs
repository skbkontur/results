using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Plain.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Result<int?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Result<int?>.Succeed(), false),
            CreateReturnBooleanCase(Result<int?>.Fail(null), true),
            CreateReturnBooleanCase(Result<int?>.Fail(1), true),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<int?> result)
        {
            return result.TryGetFault(out _);
        }

        private static TestCaseData CreateGetCase(int? expectedValue)
        {
            return new(Result<int?>.Fail(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase(10),
        };

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Fault(Result<int?> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Fault_With_If(Result<int?> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
