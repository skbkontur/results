using System.Collections.Generic;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet.Fault.Class
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
            Create(StringFaultResult.Fail<string>(new("foo")), true),
            Create(StringFaultResult.Succeed("bar"), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Return_Boolean(StringFaultResult<string> result)
        {
            return result.TryGetFault(out _);
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            StringFault fault = new("foo");
            yield return new(StringFaultResult.Fail<string>(fault)) { ExpectedResult = fault };
        }

        [TestCaseSource(nameof(GetCases))]

        public StringFault? Extract_Fault(StringFaultResult<string> result)
        {
            _ = result.TryGetFault(out var fault);

            return fault;
        }

        [TestCaseSource(nameof(GetCases))]
        public StringFault Extract_Fault_With_If(StringFaultResult<string> result)
        {
            if (result.TryGetFault(out var fault))
            {
                return fault;
            }

            throw new UnreachableException();
        }
    }
}