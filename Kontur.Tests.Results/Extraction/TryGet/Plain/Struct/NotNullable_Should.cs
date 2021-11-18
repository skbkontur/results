using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Plain.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(Result<int> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<int>.Succeed(), false),
            Create(Result<int>.Fail(1), true),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(Result<int> result)
        {
            return result.TryGetFault(out _);
        }

        private static readonly TestCaseData[] GetCases =
        {
            new(Result<int>.Fail(10)) { ExpectedResult = 10 },
        };

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Fault(Result<int> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public int Extract_Fault_With_If(Result<int> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
