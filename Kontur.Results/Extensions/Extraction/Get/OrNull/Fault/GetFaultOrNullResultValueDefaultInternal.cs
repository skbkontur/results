namespace Kontur.Results
{
    internal static partial class GetFaultOrNullResultValueDefaultInternal
    {
        internal static TFault? GetFaultOrNull<TFault, TValue>(IResult<TFault, TValue> result)
            where TFault : struct
        {
            return result.Match<TFault?>(fault => fault, () => null);
        }
    }
}
