using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrDefaultOptionalExtensions
    {
        /// <summary>Extracts the value if present.</summary>
        /// <returns>The contained value, or <c>default(TValue)</c> if None.</returns>
        [Pure]
        public static TValue? GetValueOrDefault<TValue>(this IOptional<TValue> optional)
        {
            return optional.GetValueOrElse(default(TValue));
        }
    }
}