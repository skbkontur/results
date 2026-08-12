namespace Kontur.Results
{
    internal static partial class EnsureFailureResultPlainDefaultInternal
    {
        internal static void EnsureFailure<TFault>(IResult<TFault> result)
        {
            EnsureFailureResultPlainFactoryInternal.EnsureFailure(result, () => new ResultSucceedException($"No fault in {result}"));
        }
    }
}
