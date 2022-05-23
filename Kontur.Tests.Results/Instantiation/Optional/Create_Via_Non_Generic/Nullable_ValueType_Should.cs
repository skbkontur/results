using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Optional.Create_Via_Non_Generic
{
    [TestFixture]
    internal class Nullable_ValueType_Should
    {
        private static TestCaseData CreateHasValueCase(Optional<int?> optional, bool hasValue)
        {
            return Common.CreateHasValueCase(optional, hasValue);
        }

        private static readonly TestCaseData[] HasValueNullableValueTypeCases =
        {
            CreateHasValueCase(Kontur.Results.Optional.None<int?>(), false),
            CreateHasValueCase(Kontur.Results.Optional.Some<int?>(null), true),
            CreateHasValueCase(Kontur.Results.Optional.Some<int?>(10), true),
        };

        [TestCaseSource(nameof(HasValueNullableValueTypeCases))]
        public bool HasValue(Optional<int?> optional)
        {
            return optional.HasSome;
        }

        [TestCase(null, ExpectedResult = null)]
        [TestCase(10, ExpectedResult = 10)]
        public int? Store_Value(int? value)
        {
            var optional = Kontur.Results.Optional.Some(value);

            return optional.GetValueOrThrow();
        }
    }
}
