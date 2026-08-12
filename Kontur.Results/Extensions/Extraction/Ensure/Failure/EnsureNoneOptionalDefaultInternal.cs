namespace Kontur.Results
{
    internal static partial class EnsureNoneOptionalDefaultInternal
    {
        internal static void EnsureNone<TValue>(IOptional<TValue> optional)
        {
            EnsureNoneOptionalPassInternal.EnsureNone(optional, value => new ValueExistsException<TValue>(value, $"{optional} has value"));
        }
    }
}
