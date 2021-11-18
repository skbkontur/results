using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue.Fault.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(Result<int, int> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<int, int>.Fail(22), true),
            Create(Result<int, int>.Succeed(1), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Result<int, int> result)
        {
            return result.TryGetFault(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Result<int, int>.Fail(10)) { ExpectedResult = 10 },
        };

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Fault(Result<int, int> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Fault_With_If(Result<int, int> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
