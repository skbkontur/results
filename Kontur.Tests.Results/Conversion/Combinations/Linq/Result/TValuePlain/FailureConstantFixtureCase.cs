using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain
{
    internal class FailureConstantFixtureCase : IFixtureCase
    {
        public Result<string> GetResult(int value) => Result<string>.Fail("in the end");
    }
}
