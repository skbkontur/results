namespace Kontur.Results
{
    internal static partial class GetFaultOrThrowResultPlainDefaultInternal
    {
        internal static TFault GetFaultOrThrow<TFault>(IResult<TFault> result)
        {
            return GetFaultOrThrowResultPlainFactoryInternal.GetFaultOrThrow(result, () => new ResultSucceedException($"Can not get fault from {result}"));
        }
    }
}
