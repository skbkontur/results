using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain
{
    internal class SuccessFixtureCase : IFixtureCase
    {
        public Result<string> GetResult(int value) => Result<string>.Succeed();
    }
}
