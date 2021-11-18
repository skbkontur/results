using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.Enumerator
{
    [TestFixture]
    internal class Optional_Should
    {
        private static TestCaseData CreateCase(Optional<int> optional, IEnumerable<int> results)
        {
            return new(optional) { ExpectedResult = results };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Optional<int>.Some(2), new[] { 2 }),
            CreateCase(Optional<int>.None(), Enumerable.Empty<int>()),
        };

        [TestCaseSource(nameof(Cases))]
        public IEnumerable<int> Foreach_With_Type_Safety(Optional<int> optional)
        {
            foreach (var value in optional)
            {
                yield return value;
            }
        }
    }
}