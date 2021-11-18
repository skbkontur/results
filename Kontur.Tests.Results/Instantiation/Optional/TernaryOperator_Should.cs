using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.Optional
{
    [TestFixture]
    internal class TernaryOperator_Should
    {
        private const int SomeValue = 7;

        private static TestCaseData CreateCase(bool flag, Optional<int> result)
        {
            return new(flag) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(true, SomeValue),
            CreateCase(false, Kontur.Results.Optional.None()),
        };

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Create_Via_Other_Argument_Implicit_Conversion(bool flag)
        {
            var option = flag
                ? Kontur.Results.Optional.Some(SomeValue)
                : Kontur.Results.Optional.None();
            return option;
        }

        [TestCaseSource(nameof(Cases))]
        public Optional<int> Create_Via_Target_Type_Inference(bool flag)
        {
            return flag
                ? SomeValue
                : Kontur.Results.Optional.None();
        }
    }
}
