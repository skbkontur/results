namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.TValueTValue
{
    internal interface IConstantProvider<out TValue>
    {
        string GetError();

        TValue GetValue();
    }
}
