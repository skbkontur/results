using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Enumerator
{
    [TestFixture]
    internal class TValue_Should
    {
        private static TestCaseData CreateCase(Result<string, int> result, IEnumerable<int> enumerable)
        {
            return new(result) { ExpectedResult = enumerable };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<string, int>.Fail("bar"), System.Linq.Enumerable.Empty<int>()),
            CreateCase(Result<string, int>.Succeed(2), new[] { 2 }),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Foreach_With_Type_Safety(Result<string, int> result)
        {
            foreach (var value in result)
            {
                yield return value;
            }
        }
    }
}