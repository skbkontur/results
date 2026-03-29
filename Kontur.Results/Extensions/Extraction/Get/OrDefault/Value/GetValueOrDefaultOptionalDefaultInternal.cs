namespace Kontur.Results
{
    internal static partial class GetValueOrDefaultOptionalDefaultInternal
    {
        internal static TValue? GetValueOrDefault<TValue>(IOptional<TValue> optional)
        {
            return GetValueOrElseOptionalValueInternal.GetValueOrElse(optional, default(TValue));
        }
    }
}
