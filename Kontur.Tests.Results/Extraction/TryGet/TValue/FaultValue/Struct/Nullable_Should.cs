using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.FaultValue.Struct
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
            CreateReturnBooleanCase(Result<int?, int?>.Succeed(1), false),
        };

        [TestCaseSource(nameof(ReturnBooleanCases))]
        public bool Return_Boolean(Result<int?, int?> result)
        {
            return result.TryGetFault(out _, out _);
        }

        private static TestCaseData CreateFailureCase(int? expectedValue)
        {
            return new(Result<int?, int?>.Fail(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetFailureCases =
        {
            CreateFailureCase(null),
            CreateFailureCase(10),
        };

        [TestCaseSource(nameof(GetFailureCases))]
        public int? Extract_Fault(Result<int?, int?> result)
        {
            _ = result.TryGetFault(out var fault, out _);

            return fault;
        }

        [TestCaseSource(nameof(GetFailureCases))]
        public int? Extract_Fault_With_If(Result<int?, int?> result)
        {
            if (result.TryGetFault(out var fault, out _))
            {
                return fault;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateSuccessCase(int? expectedValue)
        {
            return new(Result<int?, int?>.Succeed(expectedValue)) { ExpectedResult = expectedValue };
        }

        private static readonly TestCaseData[] GetSuccessCases =
        {
            CreateSuccessCase(null),
            CreateSuccessCase(10),
        };

        [TestCaseSource(nameof(GetSuccessCases))]
        public int? Extract_Value(Result<int?, int?> result)
        {
            _ = result.TryGetFault(out _, out var value);

            return value;
        }

        [TestCaseSource(nameof(GetSuccessCases))]
        public int? Extract_Value_With_If(Result<int?, int?> result)
        {
            if (!result.TryGetFault(out _, out var value))
            {
                return value;
            }

            throw new UnreachableException();
        }

        private static TestCaseData CreateAllCase(Result<int?, int?> result, int? expected)
        {
            return new(result) { ExpectedResult = expected };
        }

        private static readonly TestCaseData[] GetAllCases =
        {
            CreateAllCase(Result<int?, int?>.Succeed(null), null),
            CreateAllCase(Result<int?, int?>.Succeed(10), 10),
            CreateAllCase(Result<int?, int?>.Fail(null), null),
            CreateAllCase(Result<int?, int?>.Fail(33), 33),
        };

        [TestCaseSource(nameof(GetAllCases))]
        public int? Extract_All_With_If(Result<int?, int?> result)
        {
            return result.TryGetFault(out var fault, out var value)
                ? fault
                : value;
        }
    }
}
