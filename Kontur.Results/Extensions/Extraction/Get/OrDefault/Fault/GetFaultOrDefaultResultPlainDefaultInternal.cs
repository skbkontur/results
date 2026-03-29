namespace Kontur.Results
{
    internal static partial class GetFaultOrDefaultResultPlainDefaultInternal
    {
        internal static TFault? GetFaultOrDefault<TFault>(IResult<TFault> result)
        {
            return GetFaultOrElseResultPlainValueInternal.GetFaultOrElse(result, default(TFault));
        }
    }
}
