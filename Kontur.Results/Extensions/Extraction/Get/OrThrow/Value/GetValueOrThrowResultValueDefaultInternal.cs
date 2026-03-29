namespace Kontur.Results
{
    internal static partial class GetValueOrThrowResultValueDefaultInternal
    {
        internal static TValue GetValueOrThrow<TFault, TValue>(IResult<TFault, TValue> result)
        {
            return GetValueOrThrowResultValuePassInternal.GetValueOrThrow(result, fault => new ResultFailedException<TFault>(fault, $"Can not get value from {result}"));
        }
    }
}
