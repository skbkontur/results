using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.TryGet
{
    internal static class Common
    {
        internal static TestCaseData CreateReturnBooleanCase<T>(StringFaultResult<T> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }
    }
}
