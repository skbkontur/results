namespace Kontur.Results
{
    internal static partial class GetValueOrNullResultValueDefaultInternal
    {
        internal static TValue? GetValueOrNull<TFault, TValue>(IResult<TFault, TValue> result)
            where TValue : struct
        {
            return result.Match<TValue?>(() => null, value => value);
        }
    }
}
