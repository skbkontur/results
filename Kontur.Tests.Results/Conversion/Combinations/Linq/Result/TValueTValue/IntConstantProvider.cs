using System.Globalization;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue
{
    internal class IntConstantProvider : IConstantProvider<int>
    {
        public string GetError() => this.GetValue().ToString(CultureInfo.InvariantCulture);

        public int GetValue() => 9999;
    }
}
