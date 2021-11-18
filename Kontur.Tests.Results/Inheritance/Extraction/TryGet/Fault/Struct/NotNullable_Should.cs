using System.Collections.Generic;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Fault.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData Create(StringFaultResult<int> result, bool success)
        {
            return Common.CreateReturnBooleanCase(result, success);
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(StringFaultResult.Fail<int>(new("failure")), true),
            Create(StringFaultResult.Succeed(1), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(StringFaultResult<int> result)
        {
            return result.TryGetFault(out _);
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            StringFault fault = new("error");
            yield return new(StringFaultResult.Fail<int>(fault)) { ExpectedResult = fault };
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault? Extract_Fault(StringFaultResult<int> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault Extract_Fault_With_If(StringFaultResult<int> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}
