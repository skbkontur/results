using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain.Create_Via_Non_Generic
{
    [TestFixture]
    internal class Nullable_ValueType_Should
    {
        private static TestCaseData CreateSuccessCase(Result<int?> result, bool success)
        {
            return Common.CreateSuccessCase(result, success);
        }

        private static readonly TestCaseData[] SuccessNullableValueTypeCases =
        {
            CreateSuccessCase(Result.Succeed<int?>(), true),
            CreateSuccessCase(Result.Fail<int?>(null), false),
            CreateSuccessCase(Result.Fail<int?>(10), false),
        };

        [TestCaseSource(nameof(SuccessNullableValueTypeCases))]
        public bool Success(Result<int?> result)
        {
            return result.Success;
        }

        [TestCase(null, ExpectedResult = null)]
        [TestCase(10, ExpectedResult = 10)]
        public int? Store_Fault(int? value)
        {
            var result = Result.Fail(value);

            return result.GetFaultOrThrow();
        }
    }
}
