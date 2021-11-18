using System.Globalization;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations
{
    internal static class ResultFactory
    {
        internal static Result<string> CreateFailure(int value)
        {
            return Result<string>.Fail(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
