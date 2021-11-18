using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue
{
    internal interface IFixtureCase
    {
        public Result<string, TValue> GetResult<TValue>(TValue value, IConstantProvider<TValue> provider);
    }
}
