namespace Kontur.Results
{
    internal static partial class GetValueOrNullOptionalDefaultInternal
    {
        internal static TValue? GetValueOrNull<TValue>(IOptional<TValue> optional)
            where TValue : struct
        {
            return optional.Match<TValue?>(() => null, value => value);
        }
    }
}
