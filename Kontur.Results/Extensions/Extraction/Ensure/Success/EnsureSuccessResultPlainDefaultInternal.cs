namespace Kontur.Results
{
    internal static partial class EnsureSuccessResultPlainDefaultInternal
    {
        internal static void EnsureSuccess<TFault>(IResult<TFault> result)
        {
            EnsureSuccessResultPlainPassInternal.EnsureSuccess(result, fault => new ResultFailedException<TFault>(fault, $"{result} is failure"));
        }
    }
}
