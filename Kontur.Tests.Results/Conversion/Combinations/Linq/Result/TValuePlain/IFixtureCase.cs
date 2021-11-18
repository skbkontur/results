using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain
{
    internal interface IFixtureCase
    {
        public Result<string> GetResult(int value);
    }
}
