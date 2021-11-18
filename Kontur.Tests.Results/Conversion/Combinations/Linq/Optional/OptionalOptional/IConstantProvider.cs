namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional
{
    internal interface IConstantProvider<out TValue>
    {
        TValue Get();
    }
}
