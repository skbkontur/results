namespace Kontur.Results
{
    internal static partial class GetValueOrDefaultResultValueDefaultInternal
    {
        internal static TValue? GetValueOrDefault<TFault, TValue>(IResult<TFault, TValue> result)
        {
            return GetValueOrElseResultValueValueInternal.GetValueOrElse(result, default(TValue));
        }
    }
}
