using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValuePlain
{
    internal class FailureVariableFixtureCase : IFixtureCase
    {
        public Result<string> GetResult(int value) => ResultFactory.CreateFailure(value);
    }
}
