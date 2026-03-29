namespace Kontur.Results
{
    internal static partial class GetFaultOrThrowResultValueDefaultInternal
    {
        internal static TFault GetFaultOrThrow<TFault, TValue>(IResult<TFault, TValue> result)
        {
            return GetFaultOrThrowResultValuePassInternal.GetFaultOrThrow(result, value => new ResultSucceedException<TValue>(value, $"Can not get fault from {result}"));
        }
    }
}
