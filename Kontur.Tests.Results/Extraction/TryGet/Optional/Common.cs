using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.Optional
{
    internal static class Common
    {
        internal static TestCaseData CreateReturnBooleanCase<T>(Optional<T> optional, bool success)
        {
            return new(optional) { ExpectedResult = success };
        }
    }
}
