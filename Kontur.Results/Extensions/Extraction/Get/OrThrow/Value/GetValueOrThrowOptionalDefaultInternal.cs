namespace Kontur.Results
{
    internal static partial class GetValueOrThrowOptionalDefaultInternal
    {
        internal static TValue GetValueOrThrow<TValue>(IOptional<TValue> optional)
        {
            return GetValueOrThrowOptionalFactoryInternal.GetValueOrThrow(optional, () => new ValueMissingException($"Can not get value from {optional}"));
        }
    }
}
