namespace Kontur.Results
{
    internal static partial class GetFaultOrNullResultPlainDefaultInternal
    {
        internal static TFault? GetFaultOrNull<TFault>(IResult<TFault> result)
            where TFault : struct
        {
            return result.Match<TFault?>(fault => fault, () => null);
        }
    }
}
