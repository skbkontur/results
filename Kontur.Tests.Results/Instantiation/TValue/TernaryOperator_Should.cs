using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue
{
    [TestFixture]
    internal class TernaryOperator_Should
    {
        private const int IntValue = 7;
        private const string StringValue = "bar";

        private static TestCaseData CreateSameTypeCase(bool flag, Result<int, int> result)
        {
            return new(flag) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] SameTypeCases =
        {
            CreateSameTypeCase(true, Result<int, int>.Succeed(IntValue)),
            CreateSameTypeCase(false, Result<int, int>.Fail(IntValue)),
        };

        [TestCaseSource(nameof(SameTypeCases))]
        public Result<int, int> Create_Fail_Using_Marker_Via_Other_Argument_Implicit_Conversion(bool flag)
        {
            var result = flag
                ? Result<int>.Succeed(IntValue)
                : Result.Fail(IntValue);
            return result;
        }

        [TestCaseSource(nameof(SameTypeCases))]
        public Result<int, int> Create_Success_Using_Marker_Via_Other_Argument_Implicit_Conversion(bool flag)
        {
            var result = flag
                ? Result.Succeed(IntValue)
                : ResultFailure<int>.Create(IntValue);
            return result;
        }

        [TestCaseSource(nameof(SameTypeCases))]
        public Result<int, int> Create_Using_Marker_Via_Target_Type_Inference(bool flag)
        {
            return flag
                ? Result.Succeed(IntValue)
                : Result.Fail(IntValue);
        }

        private static TestCaseData CreateDifferentTypeCase(bool flag, Result<int, string> result)
        {
            return new(flag) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] DifferentTypeCases =
        {
            CreateDifferentTypeCase(true, Result<int, string>.Succeed(StringValue)),
            CreateDifferentTypeCase(false, Result<int, string>.Fail(IntValue)),
        };

        [TestCaseSource(nameof(DifferentTypeCases))]
        public Result<int, string> Create_Fail_Via_Other_Argument_Implicit_Conversion(bool flag)
        {
            var result = flag
                ? Result<int>.Succeed(StringValue)
                : IntValue;
            return result;
        }

        [TestCaseSource(nameof(DifferentTypeCases))]
        public Result<int, string> Create_Success_Via_Other_Argument_Implicit_Conversion(bool flag)
        {
            var result = flag
                ? StringValue
                : ResultFailure<string>.Create(IntValue);
            return result;
        }

        [TestCaseSource(nameof(DifferentTypeCases))]
        public Result<int, string> Create_Via_Target_Type_Inference(bool flag)
        {
            return flag
                ? StringValue
                : IntValue;
        }
    }
}
