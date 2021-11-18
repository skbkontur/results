using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.TryGet.TValue
{
    internal static class Common
    {
        internal static TestCaseData CreateReturnBooleanCase<T>(Result<T, T> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }
    }
}
