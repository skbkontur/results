using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Plain.Create_Via_Non_Generic
{
    [TestFixture]
    internal class Nullable_ReferenceType
    {
        private static TestCaseData CreateSuccessCase(Result<string?> result, bool success)
        {
            return Common.CreateSuccessCase(result, success);
        }

        private static readonly TestCaseData[] SuccessNullableReferenceTypeCases =
        {
            CreateSuccessCase(Result.Succeed<string?>(), true),
            CreateSuccessCase(Result.Fail<string?>(null), false),
            CreateSuccessCase(Result.Fail<string?>("foo"), false),
        };

        [TestCaseSource(nameof(SuccessNullableReferenceTypeCases))]
        public bool Success(Result<string?> result)
        {
            return result.Success;
        }

        [TestCase(null, ExpectedResult = null)]
        [TestCase("foo", ExpectedResult = "foo")]
        public string? Store_Fault(string? value)
        {
            var result = Result.Fail(value);

            return result.GetFaultOrThrow();
        }
    }
}
