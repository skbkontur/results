using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.ImplicitOperatorBool
{
    [TestFixture]
    internal class Plain_Should
    {
        private static TestCaseData CreateCase(Result<int> result, bool success)
        {
            return new(result) { ExpectedResult = success };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Result<int>.Succeed(), true),
            CreateCase(Result<int>.Fail(10), false),
        };

        [TestCaseSource(nameof(Cases))]
        public bool Convert(Result<int> result)
        {
            return result;
        }
    }
}