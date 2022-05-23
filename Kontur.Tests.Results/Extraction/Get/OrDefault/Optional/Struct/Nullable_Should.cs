using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Optional.Struct
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Optional<int?> optional, int? result)
        {
            return new(optional) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Optional<int?>.None(), null),
            CreateCase(Optional<int?>.Some(null), null),
            CreateCase(Optional<int?>.Some(1), 1),
        };

        [TestCaseSource(nameof(Cases))]
        public int? Process_Optional(Optional<int?> optional)
        {
            return optional.GetValueOrDefault();
        }
    }
}
