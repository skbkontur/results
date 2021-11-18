using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain
{
    [TestFixture(typeof(FailureVariableFixtureCase))]
    [TestFixture(typeof(FailureConstantFixtureCase))]
    [TestFixture(typeof(SuccessFixtureCase))]
    internal abstract class LinqTestBase<TFixtureCase>
        where TFixtureCase : IFixtureCase, new()
    {
        private protected LinqTestBase()
        {
        }

        protected static readonly TFixtureCase FixtureCase = new();

        protected static Result<string> GetResult(int value) => FixtureCase.GetResult(value);
    }
}
