using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Optional.Upcast
{
    [TestFixture]
    internal class Nullable_Should
    {
        private static TestCaseData CreateCase(Optional<Child?> optional, Base? result)
        {
            return new(optional) { ExpectedResult = result };
        }

        private static IEnumerable<TestCaseData> GetCases()
        {
            yield return CreateCase(Optional<Child?>.None(), null);
            yield return CreateCase(Optional<Child?>.Some(null), null);

            Child child = new();
            yield return CreateCase(Optional<Child?>.Some(child), child);
        }

        [TestCaseSource(nameof(GetCases))]
        public Base? Process_Option(Optional<Child?> optional)
        {
            return optional.GetValueOrDefault<Base?>();
        }
    }
}
