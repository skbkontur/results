using System.Collections.Generic;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue.Implicit_Operator
{
    [TestFixture]
    internal class Convert_Should
    {
        private static TestCaseData Create(Result<int, int> source, Result<int> result)
        {
            return new(source) { ExpectedResult = result };
        }

        private static IEnumerable<TestCaseData> CreateCases()
        {
            const int expected = 10;
            yield return Create(Result<int, int>.Fail(expected), Result<int>.Fail(expected));
            yield return Create(Result<int, int>.Succeed(0), Result<int>.Succeed());
        }

        [TestCaseSource(nameof(CreateCases))]
        public Result<int> Convert(Result<int, int> result)
        {
            return result;
        }
    }
}
