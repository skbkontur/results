namespace Kontur.Results
{
    internal static partial class EnsureHasValueOptionalDefaultInternal
    {
        internal static void EnsureHasValue<TValue>(IOptional<TValue> optional)
        {
            EnsureHasValueOptionalFactoryInternal.EnsureHasValue(optional, () => new ValueMissingException($"No value in {optional}"));
        }
    }
}
