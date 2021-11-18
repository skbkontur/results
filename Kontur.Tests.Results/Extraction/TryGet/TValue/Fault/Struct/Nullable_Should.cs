using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Fault.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateReturnBooleanCase(Result<int?, int?> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] ReturnBooleanCases =
        {
            CreateReturnBooleanCase(Result<int?, int?>.Fail(null), true),
            CreateReturnBooleanCase(Result<int?, int?>.Fail(33), true),
            CreateReturnBooleanCase(Result<int?, int?>.Succeed(null), false),
            CreateReturnBooleanCase(Result<int?, int?>.Succeed(33), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<int?, int?> result)
        {
            return result.TryGetFault(out _);
        }

        private static TestCaseData CreateGetCase(int? expectedFault)
        {
            return new(Result<int?, int?>.Fail(expectedFault)) { ExpectedResult = expectedFault };
        }

        private static readonly TestCaseData[] GetCases =
        {
            CreateGetCase(null),
            CreateGetCase(10),
        };

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Fault(Result<int?, int?> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public int? Extract_Fault_With_If(Result<int?, int?> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
