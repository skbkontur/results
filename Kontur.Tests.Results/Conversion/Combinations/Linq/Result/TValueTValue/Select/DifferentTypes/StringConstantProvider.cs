namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue.Select.DifferentTypes
{
    internal class StringConstantProvider : IConstantProvider<string>
    {
        public string GetError() => GetValue();

        public string GetValue() => "constant";
    }
}
