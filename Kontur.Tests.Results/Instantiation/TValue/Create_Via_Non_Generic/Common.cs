using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue.Create_Via_Non_Generic
{
    internal static class Common
    {
        internal static TestCaseData CreateHasValueCase<TFault, TValue>(Result<TFault, TValue> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }
    }
}
