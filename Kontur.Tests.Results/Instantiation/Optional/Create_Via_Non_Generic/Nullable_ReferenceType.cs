using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Optional.Create_Via_Non_Generic
{
    [TestFixture]
    internal class Nullable_ReferenceType
    {
        private static TestCaseData CreateHasValueCase(Optional<string?> optional, bool hasValue)
        {
            return Common.CreateHasValueCase(optional, hasValue);
        }

        private static readonly TestCaseData[] HasValueNullableReferenceTypeCases =
        {
            CreateHasValueCase(Kontur.Results.Optional.None<string?>(), false),
            CreateHasValueCase(Kontur.Results.Optional.Some<string?>(null), true),
            CreateHasValueCase(Kontur.Results.Optional.Some<string?>("foo"), true),
        };

        [TestCaseSource(nameof(HasValueNullableReferenceTypeCases))]
        public bool HasValue(Optional<string?> optional)
        {
            return optional.HasSome;
        }

        [TestCase(null, ExpectedResult = null)]
        [TestCase("foo", ExpectedResult = "foo")]
        public string? Store_Value(string? value)
        {
            var optional = Kontur.Results.Optional.Some(value);

            return optional.GetValueOrThrow();
        }
    }
}
