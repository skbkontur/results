namespace Kontur.Results
{
    internal static partial class EnsureSuccessResultValueDefaultInternal
    {
        internal static void EnsureSuccess<TFault, TValue>(IResult<TFault, TValue> result)
        {
            EnsureSuccessResultValuePassInternal.EnsureSuccess(result, fault => new ResultFailedException<TFault>(fault, $"{result} is failure"));
        }
    }
}
