using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain
{
    [TestFixture]
    internal class TernaryOperator_Should
    {
        private const int FaultValue = 7;

        private static TestCaseData CreateCase(bool flag, Result<int> result)
        {
            return new(flag) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(true, FaultValue),
            CreateCase(false, Result.Succeed()),
        };

        [TestCaseSource(nameof(Cases))]
        public Result<int> Create_Via_Other_Argument_Implicit_Conversion(bool flag)
        {
            var result = flag
                ? Result<int>.Fail(FaultValue)
                : Result.Succeed();
            return result;
        }

        [TestCaseSource(nameof(Cases))]
        public Result<int> Create_Via_Target_Type_Inference(bool flag)
        {
            return flag
                ? FaultValue
                : Result.Succeed();
        }
    }
}
