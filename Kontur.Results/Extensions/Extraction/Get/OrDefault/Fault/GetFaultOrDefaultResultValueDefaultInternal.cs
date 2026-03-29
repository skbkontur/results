namespace Kontur.Results
{
    internal static partial class GetFaultOrDefaultResultValueDefaultInternal
    {
        internal static TFault? GetFaultOrDefault<TFault, TValue>(IResult<TFault, TValue> result)
        {
            return GetFaultOrElseResultValueValueInternal.GetFaultOrElse(result, default(TFault));
        }
    }
}
