namespace Kontur.Results
{
    internal static partial class EnsureFailureResultValueDefaultInternal
    {
        internal static void EnsureFailure<TFault, TValue>(IResult<TFault, TValue> result)
        {
            EnsureFailureResultValuePassInternal.EnsureFailure(result, value => new ResultSucceedException<TValue>(value, $"No fault in {result}"));
        }
    }
}
