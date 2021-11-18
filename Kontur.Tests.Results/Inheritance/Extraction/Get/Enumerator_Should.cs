using System.Collections.Generic;
#pragma warning disable S1128 // False positive. Unused "using" should be removed
using Kontur.Results;
#pragma warning restore S1128 // Unused "using" should be removed
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get
{
    [TestFixture]
    internal class Enumerator_Should
    {
        private static TestCaseData CreateCase(StringFaultResult<int> result, IEnumerable<int> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(StringFaultResult.Fail<int>(new("bar")), System.Linq.Enumerable.Empty<int>()),
            CreateCase(StringFaultResult.Succeed(2), new[] { 2 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Foreach_With_Type_Safety(StringFaultResult<int> result)
        {
            foreach (var value in result)
            {
                yield return value;
            }
        }
    }
}