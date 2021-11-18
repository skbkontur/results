using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Optional.Class
{
    [TestFixture]
    internal class NonNullable_Should
    {
        private static TestCaseData CreateCase(Optional<string> optional, string? result)
        {
            return new(optional) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Optional<string>.None(), null),
            CreateCase(Optional<string>.Some("foo"), "foo"),
        };

        [TestCaseSource(nameof(Cases))]
        public string? Process_Option(Optional<string> optional)
        {
            return optional.GetValueOrDefault();
        }
    }
}
