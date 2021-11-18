using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Optional.Create_Via_Non_Generic
{
    internal static class Common
    {
        internal static TestCaseData CreateHasValueCase<TValue>(Optional<TValue> optional, bool hasValue)
        {
            return new(optional) { ExpectedResult = hasValue };
        }
    }
}
