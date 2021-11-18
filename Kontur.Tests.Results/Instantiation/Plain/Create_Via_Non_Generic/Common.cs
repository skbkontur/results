using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain.Create_Via_Non_Generic
{
    internal static class Common
    {
        internal static TestCaseData CreateSuccessCase<TFault>(Result<TFault> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }
    }
}
